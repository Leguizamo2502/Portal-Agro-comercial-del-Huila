using Business.Services.BackgroundServices.Options;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Orders;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Services.BackgroundServices.Implements
{
    public sealed class ExpireAwaitingPaymentBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpireAwaitingPaymentBackgroundService> _logger;
        private readonly ExpireAwaitingPaymentJobOptions _opts;

        public ExpireAwaitingPaymentBackgroundService(
        IServiceScopeFactory scopeFactory,
        IOptions<ExpireAwaitingPaymentJobOptions> opts,
        ILogger<ExpireAwaitingPaymentBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _opts = opts.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromSeconds(Math.Max(30, _opts.ScanIntervalSeconds));
            var timer = new PeriodicTimer(interval);

            _logger.LogInformation("ExpireAwaitingPayment job iniciado. Intervalo={Interval}s, BatchSize={Batch}",
                _opts.ScanIntervalSeconds, _opts.BatchSize);

            try
            {
                while (!stoppingToken.IsCancellationRequested &&
                       await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        await RunOnceAsync(stoppingToken);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        // apagando
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error ejecutando ciclo de expiración de órdenes");
                    }
                }
            }
            finally
            {
                _logger.LogInformation("ExpireAwaitingPayment job detenido.");
            }
        }


        private async Task RunOnceAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailSvc = scope.ServiceProvider.GetRequiredService<Utilities.Messaging.Interfaces.IOrderEmailService>();
            var logger = _logger;

            var now = DateTime.UtcNow;

            // 1) Selecciona candidatos en lote (solo IDs para reducir payload)
            var candidateIds = await db.Set<Order>()
                .AsNoTracking()
                .Where(o => !o.IsDeleted && o.Active
                            && o.Status == OrderStatus.AcceptedAwaitingPayment
                            && o.PaymentImageUrl == null
                            && o.AutoCloseAt != null
                            && o.AutoCloseAt <= now)
                .OrderBy(o => o.AutoCloseAt)
                .Select(o => o.Id)
                .Take(_opts.BatchSize)
                .ToListAsync(ct);

            if (candidateIds.Count == 0) return;

            int expiredCount = 0;

            // 2) Procesa cada orden con relectura y guardas (idempotente)
            foreach (var orderId in candidateIds)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    using var scopeItem = _scopeFactory.CreateScope();
                    var dbItem = scopeItem.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var emailItem = scopeItem.ServiceProvider.GetRequiredService<Utilities.Messaging.Interfaces.IOrderEmailService>();

                    // Releer con tracking para control de concurrencia
                    var order = await dbItem.Set<Entity.Domain.Models.Implements.Orders.Order>()
                        .FirstOrDefaultAsync(o => o.Id == orderId, ct);

                    if (order is null) continue; // ya no existe
                    if (order.IsDeleted || !order.Active) continue;

                    // Guardas anti-race: si cambió el estado o ya subieron comprobante, saltar
                    if (order.Status != OrderStatus.AcceptedAwaitingPayment) continue;
                    if (!string.IsNullOrWhiteSpace(order.PaymentImageUrl)) continue;
                    if (order.AutoCloseAt == null || order.AutoCloseAt > DateTime.UtcNow) continue;

                    // Transición a Expired
                    order.Status = OrderStatus.Expired;
                    // Mantén AutoCloseAt como audit trail; no lo borres
                    // Opcional: marca quién hizo el cambio (si llevas auditoría de usuario “system”)

                    await dbItem.SaveChangesAsync(ct);
                    expiredCount++;

                    // Notificar (best-effort)
                    if (_opts.SendEmails)
                    {
                        try
                        {
                            // Necesitas email del usuario: puedes resolverlo vía tu repositorio de usuarios/contactos
                            var userRepo = scopeItem.ServiceProvider.GetRequiredService<Data.Interfaces.Implements.Auth.IUserRepository>();
                            var userContact = await userRepo.GetContactUser(order.UserId);
                            if (userContact != null)
                            {
                                await emailItem.SendOrderExpiredByNoPaymentToCustomer(
                                    emailReceptor: userContact.Email,
                                    orderId: order.Id,
                                    productName: order.ProductNameSnapshot,
                                    quantityRequested: order.QuantityRequested,
                                    total: order.Total,
                                    expiredAtUtc: DateTime.UtcNow
                                );
                            }
                        }
                        catch (Exception exEmail)
                        {
                            logger.LogError(exEmail, "Error enviando correo de expiración para OrderId {OrderId}", order.Id);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Otro proceso/instancia la tocó; ignorar
                }
                catch (Exception exItem)
                {
                    _logger.LogError(exItem, "Error expirando OrderId {OrderId}", orderId);
                }
            }

            _logger.LogInformation("Job expiró {Count} órdenes (lote {Batch})", expiredCount, candidateIds.Count);
        }

    }
}

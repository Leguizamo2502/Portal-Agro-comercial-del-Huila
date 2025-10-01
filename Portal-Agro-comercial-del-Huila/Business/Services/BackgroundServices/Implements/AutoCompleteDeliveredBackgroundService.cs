using Business.Services.BackgroundServices.Options;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Producers;
using Entity.Domain.Enums;
using Entity.Domain.Models.Implements.Orders;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utilities.Messaging.Interfaces;

namespace Business.Services.BackgroundServices.Implements
{
    public sealed class AutoCompleteDeliveredBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AutoCompleteDeliveredBackgroundService> _logger;
        private readonly AutoCompleteDeliveredJobOptions _opts;

        public AutoCompleteDeliveredBackgroundService(
            IServiceScopeFactory scopeFactory,
            IOptions<AutoCompleteDeliveredJobOptions> opts,
            ILogger<AutoCompleteDeliveredBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _opts = opts.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromSeconds(Math.Max(30, _opts.ScanIntervalSeconds));
            var timer = new PeriodicTimer(interval);
            _logger.LogInformation("AutoCompleteDelivered job iniciado.");

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                try { await RunOnceAsync(stoppingToken); }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { }
                catch (Exception ex) { _logger.LogError(ex, "Error en ciclo de autocierre"); }
            }
        }

        private async Task RunOnceAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailSvc = scope.ServiceProvider.GetRequiredService<IOrderEmailService>();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var producerRepo = scope.ServiceProvider.GetRequiredService<IProducerRepository>();

            var now = DateTime.UtcNow;

            var ids = await db.Set<Order>()
                .AsNoTracking()
                .Where(o => !o.IsDeleted && o.Active
                            && o.Status == OrderStatus.DeliveredPendingBuyerConfirm
                            && o.AutoCloseAt != null && o.AutoCloseAt <= now)
                .OrderBy(o => o.AutoCloseAt)
                .Select(o => o.Id)
                .Take(_opts.BatchSize)
                .ToListAsync(ct);

            if (ids.Count == 0) return;

            var done = 0;

            foreach (var id in ids)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    using var itemScope = _scopeFactory.CreateScope();
                    var dbItem = itemScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var emailItem = itemScope.ServiceProvider.GetRequiredService<IOrderEmailService>();
                    var userRepoItem = itemScope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var producerRepoItem = itemScope.ServiceProvider.GetRequiredService<IProducerRepository>();

                    var order = await dbItem.Set<Order>().FirstOrDefaultAsync(o => o.Id == id, ct);
                    if (order == null || order.IsDeleted || !order.Active) continue;
                    if (order.Status != OrderStatus.DeliveredPendingBuyerConfirm) continue;
                    if (order.AutoCloseAt == null || order.AutoCloseAt > DateTime.UtcNow) continue;

                    // Autocierre: completar
                    order.Status = OrderStatus.Completed;
                    order.UserReceivedAnswer = UserReceivedAnswer.Yes; // autoconsentido
                    order.UserReceivedAt = DateTime.UtcNow;
                    order.AutoCloseAt = null;

                    await dbItem.SaveChangesAsync(ct);
                    done++;

                    if (_opts.SendEmails)
                    {
                        try
                        {
                            var producer = await producerRepoItem.GetContactProducer(order.ProducerIdSnapshot);
                            if (producer != null)
                            {
                                await emailItem.SendOrderCompletedToProducer(
                                    emailReceptor: producer.Email,
                                    orderId: order.Id,
                                    productName: order.ProductNameSnapshot,
                                    quantityRequested: order.QuantityRequested,
                                    total: order.Total,
                                    completedAtUtc: order.UserReceivedAt!.Value
                                );
                            }
                            // NUEVO: correo al cliente (auto-completado)
                            var customer = await userRepoItem.GetContactUser(order.UserId);
                            if (customer != null)
                            {
                                await emailItem.SendOrderCompletedToCustomer(
                                    emailReceptor: customer.Email,
                                    orderId: order.Id,
                                    productName: order.ProductNameSnapshot,
                                    quantityRequested: order.QuantityRequested,
                                    total: order.Total,
                                    completedAtUtc: order.UserReceivedAt!.Value,
                                    autoCompleted: true
                                );
                            }
                        }
                        catch (Exception exMail)
                        {
                            _logger.LogError(exMail, "Error enviando correos de autocierre OrderId {OrderId}", order.Id);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Otro proceso la tocó; ignorar
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error autocerrando OrderId {OrderId}", id);
                }
            }

            _logger.LogInformation("AutoCompleteDelivered: completadas {Count} / lote {Batch}", done, ids.Count);
        }
    }
}

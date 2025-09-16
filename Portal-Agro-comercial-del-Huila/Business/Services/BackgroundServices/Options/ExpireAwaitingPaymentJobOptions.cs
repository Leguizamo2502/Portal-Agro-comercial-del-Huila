namespace Business.Services.BackgroundServices.Options
{
    public sealed class ExpireAwaitingPaymentJobOptions
    {
        public int ScanIntervalSeconds { get; set; } = 300; // cada 5 min
        public int BatchSize { get; set; } = 200;            // cuántas órdenes por ciclo
        public bool SendEmails { get; set; } = true;         // por si quieres apagar correos
    }
}

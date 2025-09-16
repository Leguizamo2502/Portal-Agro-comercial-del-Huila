namespace Business.Services.BackgroundServices.Options
{
    public sealed class AutoCompleteDeliveredJobOptions
    {
        public int ScanIntervalSeconds { get; set; } = 300;
        public int BatchSize { get; set; } = 200;
        public bool SendEmails { get; set; } = true;
    }
}

using Business.Services.BackgroundServices.Implements;
using Business.Services.BackgroundServices.Options;

namespace Web.ProgramService
{
    public static class BackgroundServices
    {
        public static void AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
        
            //BackgroundService
            services.AddHostedService<ExpireAwaitingPaymentBackgroundService>();
            services.Configure<ExpireAwaitingPaymentJobOptions>(
                configuration.GetSection("Orders:ExpireAwaitingPaymentJob"));

            services.AddHostedService<AutoCompleteDeliveredBackgroundService>();
            services.Configure<AutoCompleteDeliveredJobOptions>(
                configuration.GetSection("Orders:AutoCompleteDeliveredJob"));

                                                                                                 
        }
    }
}

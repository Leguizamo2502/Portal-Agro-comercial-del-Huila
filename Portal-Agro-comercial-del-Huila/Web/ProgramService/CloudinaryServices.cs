using CloudinaryDotNet;

namespace Web.ProgramService
{
    public static class CloudinaryServices
    {
        public static void AddCloudinaryServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Cloudinary
            var cloudinaryConfig = configuration.GetSection("Cloudinary");

            var cloudinary = new Cloudinary(new Account(
                cloudinaryConfig["CloudName"],
                cloudinaryConfig["ApiKey"],
                cloudinaryConfig["ApiSecret"]
            ));

            services.AddSingleton(cloudinary);
        }
    }
}

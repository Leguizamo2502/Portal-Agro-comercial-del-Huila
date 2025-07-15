using Business.Interfaces.Implements;
using Business.Mapping;
using Business.Services.AuthService;
using Data.Interfaces.Implements;
using Data.Interfaces.IRepository;
using Data.Repository;
using Data.Service;
using Mapster;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Utilities.Custom;

namespace Web.ProgramService
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Mapping
            services.AddMapster();
            MapsterConfig.Register();

            //services
            services.AddScoped<EncriptePassword>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();


            services.AddScoped(typeof(IDataGeneric<>), typeof(DataGeneric<>));





            return services;
        }

    }
}

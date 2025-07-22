using Business.CustomJwt;
using Business.Interfaces.Implements;
using Business.Interfaces.Implements.Location;
using Business.Interfaces.Implements.Security;
using Business.Mapping;
using Business.Services.AuthService;
using Business.Services.Location;
using Custom.Encripter;
using Data.Interfaces.Implements;
using Data.Interfaces.Implements.Location;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Data.Repository;
using Data.Service;
using Data.Service.Location;
using Data.Service.Security;
using Mapster;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Utilities.Messaging.Implements;
using Utilities.Messaging.Interfaces;

namespace Web.ProgramService
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Email
            services.AddTransient<ISendCode, EmailService>();

            //Auth
            services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
            //services.AddScoped<EncriptePassword>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IToken, Token>();


            //Mapping
            services.AddMapster();
            MapsterConfig.Register();

            //services
           
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolUserRepository, RolUserRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IMeRepository, MeRepository>();
            services.AddScoped<IMeService, MeService>();



            services.AddScoped(typeof(IDataGeneric<>), typeof(DataGeneric<>));





            return services;
        }

    }
}

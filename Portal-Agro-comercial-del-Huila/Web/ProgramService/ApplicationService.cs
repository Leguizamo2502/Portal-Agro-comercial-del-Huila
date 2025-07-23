using Business.CustomJwt;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Location;
using Business.Interfaces.Implements.Producers;
using Business.Interfaces.Implements.Security;
using Business.Mapping;
using Business.Services.AuthService;
using Business.Services.Location;
using Business.Services.Producers;
using Custom.Encripter;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Location;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.IRepository;
using Data.Repository;
using Data.Service.Auth;
using Data.Service.Location;
using Data.Service.Producers;
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

            services.AddScoped<IFarmRepository, FarmRepository>();
            services.AddScoped<IFarmService, FarmService>();

            services.AddScoped<IProducerRepository, ProducerRepository>();





            services.AddScoped(typeof(IDataGeneric<>), typeof(DataGeneric<>));





            return services;
        }

    }
}

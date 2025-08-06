using Business.CustomJwt;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Location;
using Business.Interfaces.Implements.Producers.Categories;
using Business.Interfaces.Implements.Producers.Cloudinary;
using Business.Interfaces.Implements.Producers.Farms;
using Business.Interfaces.Implements.Producers.Products;
using Business.Interfaces.Implements.Security;
using Business.Interfaces.Implements.Security.Mes;
using Business.Mapping;
using Business.Services.AuthService;
using Business.Services.Location;
using Business.Services.Producers.Categories;
using Business.Services.Producers.Cloudinary;
using Business.Services.Producers.Farms;
using Business.Services.Producers.Products;
using Business.Services.Security;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Location;
using Data.Interfaces.Implements.Producers;
using Data.Interfaces.Implements.Producers.Categories;
using Data.Interfaces.Implements.Producers.Farms;
using Data.Interfaces.Implements.Producers.Products;
using Data.Interfaces.Implements.Security;
using Data.Interfaces.Implements.Security.Mes;
using Data.Interfaces.IRepository;
using Data.Repository;
using Data.Service.Auth;
using Data.Service.Location;
using Data.Service.Producers;
using Data.Service.Producers.Categories;
using Data.Service.Producers.Farms;
using Data.Service.Producers.Products;
using Data.Service.Security;
using Data.Service.Security.Mes;
using Mapster;
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IToken, Token>();

            //Cloudinary
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            //Mapping
            services.AddMapster();
            MapsterConfig.Register();

            //services
           
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IMeRepository, MeRepository>();
            services.AddScoped<IMeService, MeService>();

            services.AddScoped<IFarmRepository, FarmRepository>();
            services.AddScoped<IFarmService, FarmService>();

            services.AddScoped<IFarmImageService, FarmImageService>();
            services.AddScoped<IFarmImageRepository, FarmImageRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IRolService, RolService>();

            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IFormService, FormService>();

            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModuleService, ModuleService>();

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IFormModuleRepository, FormModuleRepository>();
            services.AddScoped<IFormModuleService, FormModuleService>();

            services.AddScoped<IRolUserRepository, RolUserRepository>();
            services.AddScoped<IRolUserService, RolUserService>();











            services.AddScoped<IProducerRepository, ProducerRepository>();


            //Data Generica
            services.AddScoped(typeof(IDataGeneric<>), typeof(DataGeneric<>));





            return services;
        }

    }
}

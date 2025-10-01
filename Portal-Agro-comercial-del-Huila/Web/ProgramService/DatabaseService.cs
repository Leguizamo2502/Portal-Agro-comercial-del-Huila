using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.ProgramService
{
    public static class DatabaseService
    {
        //public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<ApplicationDbContext>(opciones => opciones
        //      .UseSqlServer("name=DefaultConnection"));

        //    return services;
        //}


        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var sql = config.GetConnectionString("SqlServer");

            if (!string.IsNullOrWhiteSpace(sql))
            {
                services.AddDbContext<ApplicationDbContext>(opt =>
                    opt.UseSqlServer(sql, s =>
                    {
                        s.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        s.EnableRetryOnFailure();
                        s.CommandTimeout(60);
                    }));
            }


            return services;
        }
    }
}

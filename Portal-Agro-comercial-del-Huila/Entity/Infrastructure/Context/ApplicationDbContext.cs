using System.Reflection;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Products;
using Entity.Domain.Models.Implements.Security;
using Microsoft.EntityFrameworkCore;

namespace Entity.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
             .HasOne(u => u.Person)
             .WithOne(p => p.User)
             .HasForeignKey<User>(u => u.PersonId)
             .OnDelete(DeleteBehavior.Cascade); // o Restrict si no quieres borrado en cascada

            // Farm → Producer (sin cascada)
            modelBuilder.Entity<Farm>()
                .HasOne(f => f.Producer)
                .WithMany(p => p.Farms)
                .HasForeignKey(f => f.ProducerId)
                .OnDelete(DeleteBehavior.Restrict); // ⚠️ No cascade

            // Farm → City (con cascada)
            modelBuilder.Entity<Farm>()
                .HasOne(f => f.City)
                .WithMany(c => c.Farms)
                .HasForeignKey(f => f.CityId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ Permitido



            //Data init
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        //Auth
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

        //Security
        public DbSet<Rol> Rols { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolFormPermission> RolFormPermissions { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Domain.Models.Implements.Security.Module> Modules { get; set; }
        public DbSet<FormModule> FormModules { get; set; }


        //Producer
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<FarmImage> FarmImages { get; set; }

        //Product
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }


    }
}

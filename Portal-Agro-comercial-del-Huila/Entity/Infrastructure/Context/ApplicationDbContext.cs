using System.Reflection;
using Entity.Domain.Models.Implements.Auth;
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


            //Data init
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }
    }
}

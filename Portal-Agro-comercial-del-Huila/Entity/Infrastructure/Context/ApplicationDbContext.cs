using System.Reflection;
using Entity.Domain.Models.Implements.Auth;
using Entity.Domain.Models.Implements.Auth.Token;
using Entity.Domain.Models.Implements.Favorites;
using Entity.Domain.Models.Implements.Orders;
using Entity.Domain.Models.Implements.Producers;
using Entity.Domain.Models.Implements.Producers.Farms;
using Entity.Domain.Models.Implements.Producers.Products;
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
            // --- User ↔ Person (1–1) ---
            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Producer (opcional pero recomendable) ---
            // Evita producers duplicados por usuario
            modelBuilder.Entity<Producer>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            // --- Farm → Producer (1–N, sin cascada) ---
            modelBuilder.Entity<Farm>()
                .HasOne(f => f.Producer)
                .WithMany(p => p.Farms)
                .HasForeignKey(f => f.ProducerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Farm → City (tu política actual) ---
            modelBuilder.Entity<Farm>()
                .HasOne(f => f.City)
                .WithMany(c => c.Farms)
                .HasForeignKey(f => f.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Product → Producer (N–1, sin cascada) ---
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Producer)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.ProducerId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Product → Category (N–1) con navegación inversa explícita ---
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)                 // IMPORTANTE para evitar CategoryId1
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Order -----
            modelBuilder.Entity<Order>(e =>
            {
                e.Property(x => x.UnitPriceSnapshot).HasPrecision(18, 2);
                e.Property(x => x.Subtotal).HasPrecision(18, 2);
                //e.Property(x => x.DeliveryFee).HasPrecision(18, 2);
                e.Property(x => x.Total).HasPrecision(18, 2);

                // RowVersion (concurrencia optimista)
                e.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();

                // (Opcional) longitudes sugeridas para strings
                e.Property(x => x.ProductNameSnapshot).HasMaxLength(200);
                //e.Property(x => x.DeliveryFeeCurrency).HasMaxLength(8);
                e.Property(x => x.RecipientName).HasMaxLength(100);
                e.Property(x => x.ContactPhone).HasMaxLength(30);
                e.Property(x => x.AddressLine1).HasMaxLength(200);
                e.Property(x => x.AddressLine2).HasMaxLength(200);
                e.Property(x => x.AdditionalNotes).HasMaxLength(500);
                //e.Property(x => x.DeliveryNotes).HasMaxLength(500);
                e.Property(x => x.PaymentImageUrl).HasMaxLength(512);
            });

            // ----- Product -----
            modelBuilder.Entity<Product>(e =>
            {
                e.Property(x => x.Price).HasPrecision(18, 2);
            });

            // --- Category autorrelación (Parent ↔ SubCategories) ---
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Pivote N–M: ProductFarm (PK = Id) ---
            modelBuilder.Entity<ProductFarm>(pf =>
            {
                pf.HasKey(x => x.Id); // PK heredada de BaseModel

                pf.HasOne(x => x.Product)
                  .WithMany(p => p.ProductFarms)
                  .HasForeignKey(x => x.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);   // al borrar Product, limpia pivote

                pf.HasOne(x => x.Farm)
                  .WithMany(f => f.ProductFarms)
                  .HasForeignKey(x => x.FarmId)
                  .OnDelete(DeleteBehavior.Cascade);   // al borrar Farm, limpia pivote

                // Evitar duplicados de relación Product–Farm (respetando soft-delete)
                pf.HasIndex(x => new { x.ProductId, x.FarmId })
                  .IsUnique()
                  .HasFilter("[IsDeleted] = 0");       // Si NO usas SQL Server, elimina este HasFilter
            });

            // --- Relaciones auxiliares recomendadas (si existen las entidades) ---

            // Product → ProductImages (1–N, cascada para limpiar imágenes del producto)
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product → Favorites (1–N, cascada para limpiar favoritos al borrar producto)
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Reglas de unicidad útiles ---
            // Nombre de producto único por productor (opcional, ajusta a tu dominio)
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.ProducerId, p.Name })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            modelBuilder.Entity<Order>(b =>
            {
                b.HasIndex(o => new { o.Status, o.AutoCloseAt, o.Active, o.IsDeleted });
                b.HasOne(o => o.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(o => o.UserId)
                 .OnDelete(DeleteBehavior.Restrict); // o DeleteBehavior.NoAction en EF Core 7/8

                b.HasOne(o => o.Product)
                 .WithMany(p => p.Orders)
                 .HasForeignKey(o => o.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(o => o.City)
                 .WithMany()
                 .HasForeignKey(o => o.CityId)
                 .OnDelete(DeleteBehavior.Restrict); // opcional pero recomendable
            });

        }

        //Auth

        public DbSet<RefreshToken> RefreshToken { get; set; }
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
        public DbSet<ProducerSocialLink> ProducerSocialLinks { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<FarmImage> FarmImages { get; set; }

        //Product
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductFarm> ProductFarms { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Review> Reviews { get; set; }


        //Order
        public DbSet<Order> Orders { get; set; }
    }
}

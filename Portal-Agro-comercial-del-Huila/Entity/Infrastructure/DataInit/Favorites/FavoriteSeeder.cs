using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Favorites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Favorites
{
    public class FavoriteSeeder : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorites");                // nombre de tabla
            builder.HasKey(f => f.Id);

            builder.Property(f => f.UserId).IsRequired();
            builder.Property(f => f.ProductId).IsRequired();

            builder.HasIndex(f => new { f.UserId, f.ProductId }).IsUnique();

            // Rompe cascade para evitar "multiple cascade paths"
            builder.HasOne(f => f.User)
                   .WithMany(u => u.Favorites)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);  // o .NoAction()

            // Puedes dejar Restrict aquí también si tienes más cascadas desde Product
            builder.HasOne(f => f.Product)
                   .WithMany(p => p.Favorites)
                   .HasForeignKey(f => f.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);  // seguro para SQL Server
        }
    }
}

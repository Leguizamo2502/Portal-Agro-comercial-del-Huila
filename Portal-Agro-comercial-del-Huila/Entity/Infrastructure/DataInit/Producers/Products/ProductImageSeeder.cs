using Entity.Domain.Models.Implements.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Producers.Products
{
    public class ProductImageSeeder : IEntityTypeConfiguration<ProductImage>
    {
        

        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            var date = new DateTime(2024, 1, 1);

            builder.HasData(
               new ProductImage
               {
                   Id = 1,
                   ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png",
                   ProductId = 1,
                   CreateAt = date,
                   IsDeleted = false,
                   Active = true,
               },
                new ProductImage { Id = 2, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 2, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 3, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 3, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 4, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 4, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 5, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 5, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 6, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 6, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 7, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 7, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 8, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 8, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 9, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 9, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 10, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 10, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 11, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 11, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 12, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 12, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 13, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 13, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 14, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 14, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 15, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 15, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 16, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 16, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 17, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 17, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 18, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 18, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 19, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 19, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 20, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 20, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 21, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 21, CreateAt = date, IsDeleted = false, Active = true },
                new ProductImage { Id = 22, ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", ProductId = 22, CreateAt = date, IsDeleted = false, Active = true }

            );

        }
    }
}

using System;
using Entity.Domain.Models.Implements.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Producers
{
    public class ProductFarmSeeder : IEntityTypeConfiguration<ProductFarm>
    {
        public void Configure(EntityTypeBuilder<ProductFarm> builder)
        {
            var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new ProductFarm { Id = 1, ProductId = 1, FarmId = 1, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 2, ProductId = 2, FarmId = 1, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 3, ProductId = 3, FarmId = 1, CreateAt = date, IsDeleted = false, Active = true },

                new ProductFarm { Id = 4, ProductId = 4, FarmId = 2, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 5, ProductId = 5, FarmId = 2, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 6, ProductId = 6, FarmId = 2, CreateAt = date, IsDeleted = false, Active = true },

                new ProductFarm { Id = 7, ProductId = 7, FarmId = 3, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 8, ProductId = 8, FarmId = 3, CreateAt = date, IsDeleted = false, Active = true },

                new ProductFarm { Id = 9, ProductId = 9, FarmId = 4, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 10, ProductId = 10, FarmId = 4, CreateAt = date, IsDeleted = false, Active = true },

                new ProductFarm { Id = 11, ProductId = 11, FarmId = 1, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 12, ProductId = 12, FarmId = 2, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 13, ProductId = 13, FarmId = 3, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 14, ProductId = 14, FarmId = 4, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 15, ProductId = 15, FarmId = 2, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 16, ProductId = 16, FarmId = 3, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 17, ProductId = 17, FarmId = 1, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 18, ProductId = 18, FarmId = 4, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 19, ProductId = 19, FarmId = 2, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 20, ProductId = 20, FarmId = 4, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 21, ProductId = 21, FarmId = 3, CreateAt = date, IsDeleted = false, Active = true },
                new ProductFarm { Id = 22, ProductId = 22, FarmId = 1, CreateAt = date, IsDeleted = false, Active = true }
            );
        }
    }
}

using System;
using Entity.Domain.Models.Implements.Producers.Farms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Producers.Farms
{
    public class FarmImagesSeeder : IEntityTypeConfiguration<FarmImage>
    {
        public void Configure(EntityTypeBuilder<FarmImage> builder)
        {
            var date = new DateTime(2024, 1, 1);

            builder.HasData(
                new FarmImage
                {
                    Id = 1,
                    FarmId = 1,
                    ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg",
                    FileName = "Imagen_Default.jpg",
                    PublicId = "default",
                    CreateAt = date,
                    IsDeleted = false,
                    Active = true
                },
                new FarmImage
                {
                    Id = 2,
                    FarmId = 2,
                    ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg",
                    FileName = "Imagen_Default.jpg",
                    PublicId = "default",
                    CreateAt = date,
                    IsDeleted = false,
                    Active = true
                },
                new FarmImage
                {
                    Id = 3,
                    FarmId = 3,
                    ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg",
                    FileName = "Imagen_Default.jpg",
                    PublicId = "default",
                    CreateAt = date,
                    IsDeleted = false,
                    Active = true
                },
                new FarmImage
                {
                    Id = 4,
                    FarmId = 4,
                    ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg",
                    FileName = "Imagen_Default.jpg",
                    PublicId = "default",
                    CreateAt = date,
                    IsDeleted = false,
                    Active = true
                },
                new FarmImage
                {
                    Id = 5,
                    FarmId = 5,
                    ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg",
                    FileName = "Imagen_Default.jpg",
                    PublicId = "default",
                    CreateAt = date,
                    IsDeleted = false,
                    Active = true
                }
            );
        }
    }
}

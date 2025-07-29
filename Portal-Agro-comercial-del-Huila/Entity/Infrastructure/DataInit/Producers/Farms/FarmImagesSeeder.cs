using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Producers;
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
                   ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1753647812/Imagen_de_WhatsApp_2025-07-27_a_las_15.22.45_14c80001_uid9qb.jpg",
                   FarmId = 1,
                   CreateAt = date,
                   IsDeleted = false,
                   Active = true,
               },
               new FarmImage
               {
                   Id = 2,
                   ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1753647812/Imagen_de_WhatsApp_2025-07-27_a_las_15.22.45_14c80001_uid9qb.jpg",
                   FarmId = 2,
                   CreateAt = date,
                   IsDeleted = false,
                   Active = true,
               },
               new FarmImage
               {
                   Id = 3,
                   ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1753647812/Imagen_de_WhatsApp_2025-07-27_a_las_15.22.45_14c80001_uid9qb.jpg",
                   FarmId = 3,
                   CreateAt = date,
                   IsDeleted = false,
                   Active = true,
               },
               new FarmImage
               {
                   Id = 4,
                   ImageUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1753647812/Imagen_de_WhatsApp_2025-07-27_a_las_15.22.45_14c80001_uid9qb.jpg",
                   FarmId = 4,
                   CreateAt = date,
                   IsDeleted = false,
                   Active = true,
               }
            );
        }
    }
}

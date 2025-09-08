using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utilities.Helpers.Code;

namespace Entity.Infrastructure.DataInit.Producers
{
    public class ProducerSeeder : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.HasData(

                new Producer
                {
                    Id = 1,
                    Code = "M3QPD6Y8ZR",
                    Description = "Hola vendo papa",
                    UserId = 3,
                    QrUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1756782308/qr_png_e6xgom.png",
                    Active = true,
                    IsDeleted = false,
                    CreateAt = new DateTime(2025, 1, 1)
                },
                new Producer
                {
                    Id = 2,
                    Code = "AB7KX92TQF",
                    Description = "Hola vendo papa modo admin",
                    UserId = 1,
                    QrUrl = "https://res.cloudinary.com/djj163sc9/image/upload/v1756782308/qr_png_e6xgom.png",
                    Active = true,
                    IsDeleted = false,
                    CreateAt = new DateTime(2025, 1, 1)
                }
                );
        }
    }
}

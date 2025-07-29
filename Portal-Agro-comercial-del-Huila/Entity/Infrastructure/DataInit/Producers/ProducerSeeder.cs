using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                    Code = "PENDIENTE",
                    Description= "Hola vendo papa",
                    UserId = 3,
                    Active = true,
                    IsDeleted = true,
                    CreateAt = new DateTime(2025, 1, 1)
                }
                );
        }
    }
}

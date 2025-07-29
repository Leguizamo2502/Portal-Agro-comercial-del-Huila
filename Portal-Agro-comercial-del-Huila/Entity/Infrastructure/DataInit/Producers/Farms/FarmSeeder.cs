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
    public class FarmSeeder : IEntityTypeConfiguration<Farm>
    {

        public void Configure(EntityTypeBuilder<Farm> builder)
        {
            var date = new DateTime(2024, 1, 1);

            builder.HasData(
                new Farm
                {
                    Id = 1,
                    Name = "Finca el Jardin",
                    Hectares = 4,
                    Altitude = 1600,
                    Latitude = 1200,
                    Longitude = 600,
                    ProducerId = 1,
                    CityId = 33,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date
                },
                new Farm
                {
                    Id = 2,
                    Name = "Finca el Mirador",
                    Hectares = 4,
                    Altitude = 1600,
                    Latitude = 1200,
                    Longitude = 600,
                    ProducerId = 1,
                    CityId = 33,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date
                },
                new Farm
                {
                    Id = 3,
                    Name = "Finca los Alpes",
                    Hectares = 4,
                    Altitude = 1600,
                    Latitude = 1200,
                    Longitude = 600,
                    ProducerId = 1,
                    CityId = 33,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date
                },
                new Farm
                {
                    Id = 4,
                    Name = "Finca los Lulos",
                    Hectares = 4,
                    Altitude = 1600,
                    Latitude = 1200,
                    Longitude = 600,
                    ProducerId = 1,
                    CityId = 33,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date
                }
            );
        }
    }
}

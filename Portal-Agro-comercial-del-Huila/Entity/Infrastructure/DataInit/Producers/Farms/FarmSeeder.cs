using Entity.Domain.Models.Implements.Producers; // Farm está en este namespace
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Utilities.Helpers.Code;

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
                    Latitude = 1200.0,
                    Longitude = 600.0,
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
                    Latitude = 1200.0,
                    Longitude = 600.0,
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
                    Latitude = 1200.0,
                    Longitude = 600.0,
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
                    Latitude = 1200.0,
                    Longitude = 600.0,
                    ProducerId = 1,
                    CityId = 33,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date
                },
                new Farm
                {
                    Id = 5,
                    Name = "Finca los Primos",
                    Hectares = 4,
                    Altitude = 1600,
                    Latitude = 1200.0,
                    Longitude = 600.0,
                    ProducerId = 2,
                    CityId = 33,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date
                }
            );
        }
    }
}

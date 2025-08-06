using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Auth
{
    public class PersonSeeder : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            var initialDate = new DateTime(2025, 01, 01);

            builder.HasData(
                new Person
                {
                    Id = 1,
                    FirstName = "Persona1",
                    LastName = "Persona1",
                    Identification = "000000000",
                    CityId = 33,
                    Address = "Calle 1 # 1-1",
                    PhoneNumber = "3000000000",
                    CreateAt = initialDate,
                    IsDeleted = false
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Persona2",
                    LastName = "Persona2",
                    Identification = "000000001",
                    CityId = 34,
                    Address = "Carrera 10 # 20-15",
                    PhoneNumber = "3000000001",
                    CreateAt = initialDate,
                    IsDeleted = false
                },
                new Person
                {
                    Id = 3,
                    FirstName = "Persona3",
                    LastName = "Persona3",
                    Identification = "000000002",
                    CityId = 35,
                    Address = "Avenida 3 # 5-30",
                    PhoneNumber = "3000000003",
                    CreateAt = initialDate,
                    IsDeleted = false
                }
            );
        }
    }
}
    
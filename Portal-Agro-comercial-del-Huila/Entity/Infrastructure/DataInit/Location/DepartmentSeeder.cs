using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Location
{
    public class DepartmentSeeder : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(

                new Department { Id = 1, Name = "Amazonas" },
                new Department { Id = 2, Name = "Antioquia" },
                new Department { Id = 3, Name = "Arauca" },
                new Department { Id = 4, Name = "Atlántico" },
                new Department { Id = 5, Name = "Bolívar" },
                new Department { Id = 6, Name = "Boyacá" },
                new Department { Id = 7, Name = "Caldas" },
                new Department { Id = 8, Name = "Caquetá" },
                new Department { Id = 9, Name = "Casanare" },
                new Department { Id = 10, Name = "Cauca" },
                new Department { Id = 11, Name = "Cesar" },
                new Department { Id = 12, Name = "Chocó" },
                new Department { Id = 13, Name = "Córdoba" },
                new Department { Id = 14, Name = "Cundinamarca" },
                new Department { Id = 15, Name = "Guainía" },
                new Department { Id = 16, Name = "Guaviare" },
                new Department { Id = 17, Name = "Huila" },
                new Department { Id = 18, Name = "La Guajira" },
                new Department { Id = 19, Name = "Magdalena" },
                new Department { Id = 20, Name = "Meta" },
                new Department { Id = 21, Name = "Nariño" },
                new Department { Id = 22, Name = "Norte de Santander" },
                new Department { Id = 23, Name = "Putumayo" },
                new Department { Id = 24, Name = "Quindío" },
                new Department { Id = 25, Name = "Risaralda" },
                new Department { Id = 26, Name = "San Andrés y Providencia" },
                new Department { Id = 27, Name = "Santander" },
                new Department { Id = 28, Name = "Sucre" },
                new Department { Id = 29, Name = "Tolima" },
                new Department { Id = 30, Name = "Valle del Cauca" },
                new Department { Id = 31, Name = "Vaupés" },
                new Department { Id = 32, Name = "Vichada" }

            );
        }
    }
}

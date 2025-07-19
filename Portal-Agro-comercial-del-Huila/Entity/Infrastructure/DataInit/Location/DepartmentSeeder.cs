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
            var initialDate = new DateTime(2024, 01, 01);

            builder.HasData(
                new Department { Id = 1, Name = "Amazonas", CreateAt = initialDate },
                new Department { Id = 2, Name = "Antioquia", CreateAt = initialDate },
                new Department { Id = 3, Name = "Arauca", CreateAt = initialDate },
                new Department { Id = 4, Name = "Atlántico", CreateAt = initialDate },
                new Department { Id = 5, Name = "Bolívar", CreateAt = initialDate },
                new Department { Id = 6, Name = "Boyacá", CreateAt = initialDate },
                new Department { Id = 7, Name = "Caldas", CreateAt = initialDate },
                new Department { Id = 8, Name = "Caquetá", CreateAt = initialDate },
                new Department { Id = 9, Name = "Casanare", CreateAt = initialDate },
                new Department { Id = 10, Name = "Cauca", CreateAt = initialDate },
                new Department { Id = 11, Name = "Cesar", CreateAt = initialDate },
                new Department { Id = 12, Name = "Chocó", CreateAt = initialDate },
                new Department { Id = 13, Name = "Córdoba", CreateAt = initialDate },
                new Department { Id = 14, Name = "Cundinamarca", CreateAt = initialDate },
                new Department { Id = 15, Name = "Guainía", CreateAt = initialDate },
                new Department { Id = 16, Name = "Guaviare", CreateAt = initialDate },
                new Department { Id = 17, Name = "Huila", CreateAt = initialDate },
                new Department { Id = 18, Name = "La Guajira", CreateAt = initialDate },
                new Department { Id = 19, Name = "Magdalena", CreateAt = initialDate },
                new Department { Id = 20, Name = "Meta", CreateAt = initialDate },
                new Department { Id = 21, Name = "Nariño", CreateAt = initialDate },
                new Department { Id = 22, Name = "Norte de Santander", CreateAt = initialDate },
                new Department { Id = 23, Name = "Putumayo", CreateAt = initialDate },
                new Department { Id = 24, Name = "Quindío", CreateAt = initialDate },
                new Department { Id = 25, Name = "Risaralda", CreateAt = initialDate },
                new Department { Id = 26, Name = "San Andrés y Providencia", CreateAt = initialDate },
                new Department { Id = 27, Name = "Santander", CreateAt = initialDate },
                new Department { Id = 28, Name = "Sucre", CreateAt = initialDate },
                new Department { Id = 29, Name = "Tolima", CreateAt = initialDate },
                new Department { Id = 30, Name = "Valle del Cauca", CreateAt = initialDate },
                new Department { Id = 31, Name = "Vaupés", CreateAt = initialDate },
                new Department { Id = 32, Name = "Vichada", CreateAt = initialDate }
            );
        }
    }
}

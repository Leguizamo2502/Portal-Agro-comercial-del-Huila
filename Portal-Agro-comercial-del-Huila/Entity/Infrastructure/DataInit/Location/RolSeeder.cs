using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Location
{
    public class RolSeeder : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasData(
                new Rol
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Rol con permisos administrativos",
                    Active = true,
                    IsDeleted = false,
                    CreateAt = new DateTime(2024, 1, 1)
                },
                new Rol
                {
                    Id = 2,
                    Name = "Consumer",
                    Description = "Rol con permisos de usuario",
                    Active = true,
                    IsDeleted = false,
                    CreateAt = new DateTime(2024, 1, 1)
                },
                 new Rol
                 {
                     Id = 3,
                     Name = "Producer",
                     Description = "Rol con permisos de Productor",
                     Active = true,
                     IsDeleted = false,
                     CreateAt = new DateTime(2024, 1, 1)
                 }
            );
        }
    }
}

using Entity.Domain.Models.Implements.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Security
{
    public class PermissionSeeder : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            var created = new DateTime(2024, 1, 1);

            builder.HasData(
                new Permission { Id = 1, Name = "leer", Description = "Puede ver", Active = true, IsDeleted = false, CreateAt = created },
                new Permission { Id = 2, Name = "crear", Description = "Puede crear", Active = true, IsDeleted = false, CreateAt = created },
                new Permission { Id = 3, Name = "actualizar", Description = "Puede editar", Active = true, IsDeleted = false, CreateAt = created },
                new Permission { Id = 4, Name = "eliminar", Description = "Puede eliminar", Active = true, IsDeleted = false, CreateAt = created }
            );
        }
    }
}

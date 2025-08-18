using Entity.Domain.Models.Implements.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Security
{
    public class ModuleSeeder : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            var created = new DateTime(2024, 1, 1);

            builder.HasData(
                new Module { Id = 1, Name = "Seguridad", Description = "Administración de seguridad", Active = true, IsDeleted = false, CreateAt = created },
                new Module { Id = 2, Name = "Parámetros", Description = "Parámetros del sistema", Active = true, IsDeleted = false, CreateAt = created },
                new Module
                {
                    Id = 3,
                    Name = "Productor",
                    Description = "Gestión del productor",
                    Active = true,
                    IsDeleted = false,
                    CreateAt = created
                }
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Security
{
    public class FormSeeder : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            var created = new DateTime(2024, 1, 1);
            //builder.HasIndex(x => x.Url).IsUnique();

            builder.HasData(
                new Form { Id = 1, Name = "Formularios", Description = "Gestión de formularios", Url = "/account/security/form", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 2, Name = "Usuarios", Description = "Gestión de usuarios", Url = "/account/security/user", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 3, Name = "Roles", Description = "Gestión de roles", Url = "/account/security/rol", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 4, Name = "Módulos", Description = "Gestión de módulos", Url = "/account/security/module", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 5, Name = "Rol-Usuario", Description = "Asignación rol-usuario", Url = "/account/security/rolUser", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 6, Name = "Módulo-Formulario", Description = "Asignación módulo-form", Url = "/account/security/formModule", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 7, Name = "Rol-Formulario-Permiso", Description = "Asignación R-F-P", Url = "/account/security/rolFormPermission", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 8, Name = "Permisos", Description = "Catálogo de permisos", Url = "/account/security/permission", Active = true, IsDeleted = false, CreateAt = created },
                new Form
                {
                    Id = 9,
                    Name = "Inicio productor",
                    Description = "Puerta de acceso a la gestión del productor",
                    Url = "/account/producer",
                    Active = true,
                    IsDeleted = false,
                    CreateAt = created
                },
                new Form { Id = 10, Name = "Resumen Productor", Description = "Resumen", Url = "/account/producer/summary", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 11, Name = "Productos", Description = "Gestión de productos", Url = "/account/producer/management/product", Active = true, IsDeleted = false, CreateAt = created },
                new Form { Id = 12, Name = "Fincas", Description = "Gestión de fincas", Url = "/account/producer/management/farm", Active = true, IsDeleted = false, CreateAt = created }
            );
        }
    }
}

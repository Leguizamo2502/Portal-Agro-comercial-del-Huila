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
    public class RolFormPermissionSeeder : IEntityTypeConfiguration<RolFormPermission>
    {
        public void Configure(EntityTypeBuilder<RolFormPermission> builder)
        {
            var created = new DateTime(2024, 1, 1);

            const int R_ADMIN = 1;
            var forms = new[] { 1, 2, 3, 4, 5, 6, 7, 8,9,10,11,12 };         // todos los forms de Seguridad
            var perms = new[] { 1, 2, 3, 4 };                 // leer, crear, actualizar, eliminar

            // Índice único recomendado
            //builder.HasIndex(x => new { x.RolId, x.FormId, x.PermissionId }).IsUnique();

            var data = new List<RolFormPermission>();
            int nextId = 1;

            foreach (var f in forms)
                foreach (var p in perms)
                    data.Add(new RolFormPermission
                    {
                        Id = nextId++,
                        RolId = R_ADMIN,
                        FormId = f,
                        PermissionId = p,
                        Active = true,
                        IsDeleted = false,
                        CreateAt = created
                    });

            builder.HasData(data);
        }
    }
}

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
    public class FormModuleSeeder : IEntityTypeConfiguration<FormModule>
    {
        public void Configure(EntityTypeBuilder<FormModule> builder)
        {
            var created = new DateTime(2024, 1, 1);

            //builder.HasIndex(x => new { x.FormId, x.ModuleId }).IsUnique();

            builder.HasData(
                //Security
                new FormModule { Id = 1, ModuleId = 1, FormId = 1, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 2, ModuleId = 1, FormId = 2, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 3, ModuleId = 1, FormId = 3, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 4, ModuleId = 1, FormId = 4, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 5, ModuleId = 1, FormId = 5, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 6, ModuleId = 1, FormId = 6, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 7, ModuleId = 1, FormId = 7, Active = true, IsDeleted = false, CreateAt = created },
                new FormModule { Id = 8, ModuleId = 1, FormId = 8, Active = true, IsDeleted = false, CreateAt = created },
               //Producer
               new FormModule { Id = 9, ModuleId = 3, FormId = 9, Active = true, IsDeleted = false, CreateAt = created }, // /account/producer
                new FormModule { Id = 10, ModuleId = 3, FormId = 10, Active = true, IsDeleted = false, CreateAt = created }, // /account/producer/summary
                new FormModule { Id = 11, ModuleId = 3, FormId = 11, Active = true, IsDeleted = false, CreateAt = created }, // /account/producer/management/product
                new FormModule { Id = 12, ModuleId = 3, FormId = 12, Active = true, IsDeleted = false, CreateAt = created }  // /account/producer/management/farm
            );
        }
    }
}

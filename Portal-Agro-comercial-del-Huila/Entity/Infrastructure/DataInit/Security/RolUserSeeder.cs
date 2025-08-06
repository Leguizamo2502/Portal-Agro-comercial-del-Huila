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
    public class RolUserSeeder : IEntityTypeConfiguration<RolUser>
    {
        public void Configure(EntityTypeBuilder<RolUser> builder)
        {
            var initialDate = new DateTime(2025, 01, 01);

            builder.HasData(
                // Admin -> todos los roles
                new RolUser
                {
                    Id = 1,
                    UserId = 1, // admin@example.com
                    RolId = 1,  // Admin
                    CreateAt = initialDate,
                    Active = true,
                    IsDeleted = false
                },
                new RolUser
                {
                    Id = 2,
                    UserId = 1, // admin@example.com
                    RolId = 2,  // Consumer
                    CreateAt = initialDate,
                    Active = true,
                    IsDeleted = false
                },
                new RolUser
                {
                    Id = 3,
                    UserId = 1, // admin@example.com
                    RolId = 3,  // Producer
                    CreateAt = initialDate,
                    Active = true,
                    IsDeleted = false
                },

                // Producer -> Producer + Consumer
                new RolUser
                {
                    Id = 4,
                    UserId = 3, // producer@example.com
                    RolId = 3,  // Producer
                    CreateAt = initialDate,
                    Active = true,
                    IsDeleted = false
                },
                new RolUser
                {
                    Id = 5,
                    UserId = 3, // producer@example.com
                    RolId = 2,  // Consumer
                    CreateAt = initialDate,
                    Active = true,
                    IsDeleted = false
                },

                // Consumer -> solo Consumer
                new RolUser
                {
                    Id = 6,
                    UserId = 2, // user@example.com
                    RolId = 2,  // Consumer
                    CreateAt = initialDate,
                    Active = true,
                    IsDeleted = false
                }
            );
        }
    }
}

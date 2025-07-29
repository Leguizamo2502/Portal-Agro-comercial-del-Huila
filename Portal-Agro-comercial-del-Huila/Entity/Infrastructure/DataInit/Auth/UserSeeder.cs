
using Custom.Encripter;
using Entity.Domain.Models.Implements.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Auth
{
    public class UserSeeder : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var initialDate = new DateTime(2025, 01, 01);
            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@example.com",
                    Password = EncriptePassword.EncripteSHA256("Admin123"),
                    PersonId = 1,
                    Active = true,
                    IsDeleted = false,
                    CreateAt = initialDate,

                },
                new User
                {
                    Id = 2,
                    Email = "user@example.com",
                    Password = EncriptePassword.EncripteSHA256("User123"),
                    PersonId = 2,
                    Active = true,
                    IsDeleted = false,
                    CreateAt = initialDate
                },
                new User
                {
                    Id = 3,
                    Email = "producer@example.com",
                    Password = EncriptePassword.EncripteSHA256("producer123"),
                    PersonId = 3,
                    Active = true,
                    IsDeleted = false,
                    CreateAt = initialDate
                }
            );

        }
    }
}

using Data.Service.Security;
using Entity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace UnitTest.Rol
{
    public class RolRepositoryTests
    {
        private static ApplicationDbContext BuildContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldPersistEntity()
        {
            // Arrange
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var repo = new RolRepository(ctx);
            var rol = new Entity.Domain.Models.Implements.Security.Rol { Name = "Admin", Description = "A" };

            // Act
            var created = await repo.AddAsync(rol);

            // Assert
            var total = await ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().CountAsync();
            Assert.Equal(1, total);
            Assert.Equal("Admin", created.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnly_NotDeleted()
        {
            // Arrange
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().AddRange(
                new Entity.Domain.Models.Implements.Security.Rol { Name = "A", Description = "A", IsDeleted = false },
                new Entity.Domain.Models.Implements.Security.Rol { Name = "B", Description = "B", IsDeleted = true },
                new Entity.Domain.Models.Implements.Security.Rol { Name = "C", Description = "C", IsDeleted = false }
            );
            await ctx.SaveChangesAsync();
            var repo = new RolRepository(ctx);

            // Act
            var list = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, list.Count());
            Assert.DoesNotContain(list, x => x.Name == "B");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenNotDeleted()
        {
            // Arrange
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var active = new Entity.Domain.Models.Implements.Security.Rol { Name = "QA", Description = "X", IsDeleted = false };
            ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().Add(active);
            await ctx.SaveChangesAsync();
            var repo = new RolRepository(ctx);

            // Act
            var found = await repo.GetByIdAsync(active.Id);

            // Assert
            Assert.NotNull(found);
            Assert.Equal("QA", found!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenDeletedLogically()
        {
            // Arrange
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var deleted = new Entity.Domain.Models.Implements.Security.Rol { Name = "Old", Description = "Y", IsDeleted = true };
            ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().Add(deleted);
            await ctx.SaveChangesAsync();
            var repo = new RolRepository(ctx);

            // Act
            var found = await repo.GetByIdAsync(deleted.Id);

            // Assert
            Assert.Null(found);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateValues_AndReturnTrue()
        {
            // Arrange
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var original = new Entity.Domain.Models.Implements.Security.Rol { Name = "Producer", Description = "P" };
            ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().Add(original);
            await ctx.SaveChangesAsync();
            var repo = new RolRepository(ctx);

            var toUpdate = new Entity.Domain.Models.Implements.Security.Rol { Id = original.Id, Name = "Producer-EDIT", Description = "Edited" };

            // Act
            var ok = await repo.UpdateAsync(toUpdate);

            // Assert
            Assert.True(ok);
            var reloaded = await ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().FindAsync(original.Id);
            Assert.Equal("Producer-EDIT", reloaded!.Name);
            Assert.Equal("Edited", reloaded.Description);
        }

        [Fact]
        public async Task DeleteLogicAsync_ShouldMarkIsDeleted_AndHideFromGetAll()
        {
            // Arrange
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var r = new Entity.Domain.Models.Implements.Security.Rol { Name = "Temp", Description = "T" };
            ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().Add(r);
            await ctx.SaveChangesAsync();
            var repo = new RolRepository(ctx);

            // Act
            var ok = await repo.DeleteLogicAsync(r.Id);

            // Assert
            Assert.True(ok);

            // ya no debe aparecer en GetAll (filtra IsDeleted)
            var list = await repo.GetAllAsync();
            Assert.Empty(list);

            // la fila existe pero con IsDeleted = true
            var raw = await ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().AsNoTracking().FirstAsync(x => x.Id == r.Id);
            Assert.True(raw.IsDeleted);
        }

        // Opcional (si quieres cubrir hard delete positivo/negativo):
        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenEntityExists()
        {
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var r = new Entity.Domain.Models.Implements.Security.Rol { Name = "X" , Description="Xd"};
            ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().Add(r);
            await ctx.SaveChangesAsync();
            var repo = new RolRepository(ctx);

            var ok = await repo.DeleteAsync(r.Id);

            Assert.True(ok);
            Assert.Equal(0, await ctx.Set<Entity.Domain.Models.Implements.Security.Rol>().CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenEntityNotFound()
        {
            using var ctx = BuildContext(Guid.NewGuid().ToString());
            var repo = new RolRepository(ctx);

            var ok = await repo.DeleteAsync(999);

            Assert.False(ok);
        }
    }
}

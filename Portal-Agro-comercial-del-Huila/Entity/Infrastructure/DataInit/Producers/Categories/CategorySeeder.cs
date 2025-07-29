using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Implements.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Producers.Categories
{
    public class CategorySeeder : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            var date = new DateTime(2025, 1, 1);
            builder.HasData(
            // Nivel 1 (Raíz)
            new Category { Id = 1, Name = "Frutas", IsDeleted = false, CreateAt = date, Active = true },

            // Nivel 2
            new Category { Id = 2, Name = "Cítricos", IsDeleted = false, CreateAt = date, Active = true, ParentCategoryId = 1 },
            new Category { Id = 3, Name = "Tropicales", IsDeleted = false, CreateAt = date, Active = true, ParentCategoryId = 1 },

            // Nivel 3
            new Category { Id = 4, Name = "Exóticas", IsDeleted = false, CreateAt = date, Active = true, ParentCategoryId = 3 },

            // Otro grupo raíz
            new Category { Id = 5, Name = "Hortalizas", IsDeleted = false, CreateAt = date, Active = true },
            new Category { Id = 6, Name = "Tubérculos", IsDeleted = false, CreateAt = date, Active = true, ParentCategoryId = 5 },

            // Otra rama
            new Category { Id = 7, Name = "Granos", IsDeleted = false, CreateAt = date, Active = true },
            new Category { Id = 8, Name = "Café", IsDeleted = false, CreateAt = date, Active = true, ParentCategoryId = 7 },
            new Category { Id = 9, Name = "Cacao", IsDeleted = false, CreateAt = date, Active = true, ParentCategoryId = 7 }
            );
        }
    }
}

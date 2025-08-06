using Entity.Domain.Models.Implements.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.Infrastructure.DataInit.Producers.Products
{
    public class ProductSeeder : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            var date = new DateTime(2024, 1, 1);
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Name = "Cafe el sabor",
                    Description = "Cafe con el mejor sabor del campo",
                    Price = 30000,
                    Unit = "lb",
                    Production = "300 lb cada tres meses",
                    Stock = 250,
                    Status = true,
                    CategoryId = 8,
                    FarmId = 1,
                    IsDeleted = false,
                    Active = true,
                    CreateAt = date

                },
                new Product { Id = 2, Name = "Café Orgánico Premium", Description = "Cultivado sin químicos, sabor intenso", Price = 35000, Unit = "lb", Production = "200 lb por trimestre", Stock = 180, Status = true, CategoryId = 8, FarmId = 1,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 3, Name = "Café Tostado Suave", Description = "Tueste medio con notas frutales", Price = 32000, Unit = "lb", Production = "150 lb cada mes", Stock = 120, Status = true, CategoryId = 8, FarmId = 1,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 4, Name = "Café Grano Oscuro", Description = "Grano seleccionado de alta montaña", Price = 34000, Unit = "lb", Production = "180 lb bimestral", Stock = 210, Status = true, CategoryId = 8, FarmId = 2,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 5, Name = "Café El Mirador", Description = "Cosechado a mano en clima fresco", Price = 30000, Unit = "lb", Production = "220 lb trimestral", Stock = 190, Status = true, CategoryId = 8, FarmId = 2,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 6, Name = "Café Clásico de los Andes", Description = "Sabor balanceado, aroma suave", Price = 31000, Unit = "lb", Production = "250 lb trimestral", Stock = 170, Status = true, CategoryId = 8, FarmId = 2,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 7, Name = "Café Supremo", Description = "Mezcla selecta de granos", Price = 36000, Unit = "lb", Production = "300 lb cada 2 meses", Stock = 260, Status = true, CategoryId = 8, FarmId = 3,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 8, Name = "Café los Alpes", Description = "Cultivo en altitudes extremas", Price = 37000, Unit = "lb", Production = "280 lb trimestral", Stock = 250, Status = true, CategoryId = 8, FarmId = 3,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 9, Name = "Café Lulo Blend", Description = "Mezcla con notas cítricas", Price = 33000, Unit = "lb", Production = "230 lb bimestral", Stock = 200, Status = true, CategoryId = 8, FarmId = 4,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 10, Name = "Café del Bosque", Description = "Tueste natural, suave al paladar", Price = 30000, Unit = "lb", Production = "180 lb mensual", Stock = 160, Status = true, CategoryId = 8, FarmId = 4,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 11, Name = "Café Reserva Especial", Description = "Grano seleccionado manualmente", Price = 38000, Unit = "lb", Production = "150 lb cada tres meses", Stock = 130, Status = true, CategoryId = 8, FarmId = 1,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 12, Name = "Café Sierra Verde", Description = "Cultivo bajo sombra natural", Price = 31000, Unit = "lb", Production = "200 lb cada 2 meses", Stock = 140, Status = true, CategoryId = 8, FarmId = 2,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 13, Name = "Café del Amanecer", Description = "Grano joven de excelente aroma", Price = 30500, Unit = "lb", Production = "160 lb mensual", Stock = 150, Status = true, CategoryId = 8, FarmId = 3,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 14, Name = "Café Tostado Artesanal", Description = "Tueste lento en horno de barro", Price = 34000, Unit = "lb", Production = "190 lb bimestral", Stock = 175, Status = true, CategoryId = 8, FarmId = 4,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 15, Name = "Café con Cacao", Description = "Mezcla suave con aroma a chocolate", Price = 33000, Unit = "lb", Production = "210 lb trimestral", Stock = 160, Status = true, CategoryId = 9, FarmId = 2,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 16, Name = "Café Gourmet del Campo", Description = "Sabor intenso con notas amaderadas", Price = 37000, Unit = "lb", Production = "280 lb bimestral", Stock = 210, Status = true, CategoryId = 8, FarmId = 3,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 17, Name = "Café Lavado", Description = "Proceso húmedo tradicional", Price = 32000, Unit = "lb", Production = "190 lb mensual", Stock = 160, Status = true, CategoryId = 8, FarmId = 1,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 18, Name = "Café Natural", Description = "Secado al sol directamente", Price = 31000, Unit = "lb", Production = "220 lb cada 3 meses", Stock = 180, Status = true, CategoryId = 8, FarmId = 4,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 19, Name = "Café de Altura", Description = "Granos cultivados a 1600msnm", Price = 35000, Unit = "lb", Production = "270 lb trimestral", Stock = 200, Status = true, CategoryId = 8, FarmId = 2,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 20, Name = "Café Lulo Espresso", Description = "Versión fuerte ideal para espresso", Price = 35500, Unit = "lb", Production = "160 lb mensual", Stock = 190, Status = true, CategoryId = 8, FarmId = 4,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 21, Name = "Café Cacao Fusion", Description = "Mezcla gourmet café y cacao", Price = 39000, Unit = "lb", Production = "240 lb trimestral", Stock = 150, Status = true, CategoryId = 9, FarmId = 3,
                IsDeleted = false,
                Active = true,
                CreateAt = date},
                new Product { Id = 22, Name = "Café de Exportación", Description = "Selección premium para exportación", Price = 40000, Unit = "lb", Production = "300 lb cada 2 meses", Stock = 220, Status = true, CategoryId = 8, FarmId = 1,
                IsDeleted = false,
                Active = true,
                CreateAt = date}



            );
        }
    }
}

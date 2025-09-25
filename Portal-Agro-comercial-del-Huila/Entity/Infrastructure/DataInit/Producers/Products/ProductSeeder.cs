using Entity.Domain.Models.Implements.Producers.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Utilities.Helpers.Code;

namespace Entity.Infrastructure.DataInit.Producers.Products
{
    public class ProductSeeder : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Product { Id = 1, Name = "Cafe el sabor", Code = "A7K2Q8M3ZB", Description = "Cafe con el mejor sabor del campo", Price = 30000m, Unit = "lb", Production = "300 lb cada tres meses", Stock = 250, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 2, Name = "Café Orgánico Premium", Code = "H9DN5W4R2J", Description = "Cultivado sin químicos, sabor intenso", Price = 35000m, Unit = "lb", Production = "200 lb por trimestre", Stock = 180, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 3, Name = "Café Tostado Suave", Code = "K3M7P1X6TY", Description = "Tueste medio con notas frutales", Price = 32000m, Unit = "lb", Production = "150 lb cada mes", Stock = 120, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 4, Name = "Café Grano Oscuro", Code = "Q2Z8N7L4VF", Description = "Grano seleccionado de alta montaña", Price = 34000m, Unit = "lb", Production = "180 lb bimestral", Stock = 210, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 5, Name = "Café El Mirador", Code = "M8R3C6J9PA", Description = "Cosechado a mano en clima fresco", Price = 30000m, Unit = "lb", Production = "220 lb trimestral", Stock = 190, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 6, Name = "Café Clásico de los Andes", Code = "T5Y1H7K3UE", Description = "Sabor balanceado, aroma suave", Price = 31000m, Unit = "lb", Production = "250 lb trimestral", Stock = 170, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 7, Name = "Café Supremo", Code = "V4B9Q2M6LX", Description = "Mezcla selecta de granos", Price = 36000m, Unit = "lb", Production = "300 lb cada 2 meses", Stock = 260, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 8, Name = "Café los Alpes", Code = "Z6F1D8R3NW", Description = "Cultivo en altitudes extremas", Price = 37000m, Unit = "lb", Production = "280 lb trimestral", Stock = 250, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 9, Name = "Café Lulo Blend", Code = "N2X7C5Q1JG", Description = "Mezcla con notas cítricas", Price = 33000m, Unit = "lb", Production = "230 lb bimestral", Stock = 200, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 10, Name = "Café del Bosque", Code = "R8M2T5K9WY", Description = "Tueste natural, suave al paladar", Price = 30000m, Unit = "lb", Production = "180 lb mensual", Stock = 160, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 11, Name = "Café Reserva Especial", Code = "L7P3V9H2QX", Description = "Grano seleccionado manualmente", Price = 38000m, Unit = "lb", Production = "150 lb cada tres meses", Stock = 130, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 12, Name = "Café Sierra Verde", Code = "C9J4N1Z7TR", Description = "Cultivo bajo sombra natural", Price = 31000m, Unit = "lb", Production = "200 lb cada 2 meses", Stock = 140, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 13, Name = "Café del Amanecer", Code = "X3Q8L6M2DP", Description = "Grano joven de excelente aroma", Price = 30500m, Unit = "lb", Production = "160 lb mensual", Stock = 150, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 14, Name = "Café Tostado Artesanal", Code = "J5H2K9T4VE", Description = "Tueste lento en horno de barro", Price = 34000m, Unit = "lb", Production = "190 lb bimestral", Stock = 175, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 15, Name = "Café con Cacao", Code = "P6M1X8Q3LR", Description = "Mezcla suave con aroma a chocolate", Price = 33000m, Unit = "lb", Production = "210 lb trimestral", Stock = 160, Status = true, CategoryId = 9, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 16, Name = "Café Gourmet del Campo", Code = "D2V9R5N7QH", Description = "Sabor intenso con notas amaderadas", Price = 37000m, Unit = "lb", Production = "280 lb bimestral", Stock = 210, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 17, Name = "Café Lavado", Code = "U1K7P3Z8MW", Description = "Proceso húmedo tradicional", Price = 32000m, Unit = "lb", Production = "190 lb mensual", Stock = 160, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 18, Name = "Café Natural", Code = "F4T9L2H6QY", Description = "Secado al sol directamente", Price = 31000m, Unit = "lb", Production = "220 lb cada 3 meses", Stock = 180, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 19, Name = "Café de Altura", Code = "S8N3D7V1KJ", Description = "Granos cultivados a 1600msnm", Price = 35000m, Unit = "lb", Production = "270 lb trimestral", Stock = 200, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 20, Name = "Café Lulo Espresso", Code = "Y2Q6M9P4TX", Description = "Versión fuerte ideal para espresso", Price = 35500m, Unit = "lb", Production = "160 lb mensual", Stock = 190, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 21, Name = "Café Cacao Fusion", Code = "G9L2R5X1NV", Description = "Mezcla gourmet café y cacao", Price = 39000m, Unit = "lb", Production = "240 lb trimestral", Stock = 150, Status = true, CategoryId = 9, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date },
                new Product { Id = 22, Name = "Café de Exportación", Code = "W3H8Q2M7LC", Description = "Selección premium para exportación", Price = 40000m, Unit = "lb", Production = "300 lb cada 2 meses", Stock = 220, Status = true, CategoryId = 8, ProducerId = 1, IsDeleted = false, Active = true, CreateAt = date }
            );
        }
    }
}

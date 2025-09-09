using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Entity.Domain.Models.Implements.Favorites;
using Entity.Domain.Models.Implements.Orders;
using Entity.Domain.Models.Implements.Producers.Farms;

namespace Entity.Domain.Models.Implements.Producers.Products
{
    public class Product : BaseModel
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string Unit { get; set; } = default!;
        public string Production { get; set; } = default!;
        public int Stock { get; set; }
        public bool Status { get; set; }
        public bool ShippingIncluded { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        // Nuevo: dueño explícito del producto (multi-tenant por productor)
        public int ProducerId { get; set; }
        public Producer Producer { get; set; } = default!;

        public List<ProductImage> ProductImages { get; set; } = new();
        public ICollection<Favorite> Favorites { get; set; } = [];

        // Relación N–M vía pivote
        public ICollection<ProductFarm> ProductFarms { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];

    }
}

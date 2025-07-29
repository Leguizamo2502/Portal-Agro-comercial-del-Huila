using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Producer.Categories
{
    public class CategorySelectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string? ParentName { get; set; } // opcional si necesitas mostrar el nombre de la categoría padre
    }
}

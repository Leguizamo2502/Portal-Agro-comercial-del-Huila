using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Order.Reviews
{
    public class ReviewCreateDto
    {
        public int ProductId { get; set; }
        public byte Rating { get; set; }      // 1..5
        public string Comment { get; set; }   // requerido
    }
}

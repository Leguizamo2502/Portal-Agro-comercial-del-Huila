using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
    public class Favorite
    {
        public int Id { get; set; }
        public string CreateAt { get; set; }
        public int ConsumerId { get; set; }
        public int ProductId { get; set; }
        public string ConsumerName { get; set; }
        public string ProductName { get; set; }

    }
}

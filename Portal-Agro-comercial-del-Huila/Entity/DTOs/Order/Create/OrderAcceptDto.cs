using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Order.Create
{
    public class OrderAcceptDto
    {
        public decimal DeliveryFee { get; set; }
        public string? DeliveryNotes { get; set; }
    }
}

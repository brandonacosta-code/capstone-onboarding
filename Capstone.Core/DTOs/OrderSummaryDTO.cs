using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Core.DTOs
{
    public class OrderSummaryDTO
    {
        public int OrderId { get; set; }
        public List<OrderItemDTO> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }
}

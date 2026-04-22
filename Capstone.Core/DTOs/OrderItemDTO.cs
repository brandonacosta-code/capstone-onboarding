using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Core.DTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Core.DTOs
{
    public class CreateOrderDTO
    {
        public List<OrderItemDTO> Items { get; set; }
    }
}

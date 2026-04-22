using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Core.DTOs
{
    public class ProductDTO
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal UnitPrice { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public int Stock { get; set; }

	}
}

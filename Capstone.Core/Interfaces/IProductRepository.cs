using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Core.DTOs;

namespace Capstone.Core.Interfaces
{
    public interface IProductRepository
    {
		Task<IEnumerable<ProductDTO>> GetProducts(string search, string sortBy, string sortDirection);
		Task<ProductDTO> GetProductById(int id); 
	}
}

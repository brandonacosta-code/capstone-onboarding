using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Core.DTOs;

namespace Capstone.Core.Interfaces
{
    public interface IOrderRepository
    {
		Task<int> CreateOrder(CreateOrderDTO order, decimal subTotal, decimal tax, decimal shipping, decimal total);
	}
}

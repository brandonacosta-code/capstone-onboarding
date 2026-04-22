using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Capstone.Infrastructure.Repositories;
using MediatR;

namespace Capstone.Application.CQRS.Commands
{
    public class CreateOrderCommand : IRequest<OrderSummaryDTO>
    {
        public CreateOrderDTO Order { get; set; }

        public CreateOrderCommand(CreateOrderDTO order) 
        {
            Order = order;
        }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderSummaryDTO>
    {
        private readonly IOrderRepository _orderRepository;

        private const decimal TAX_RATE = 0.05m;
        private readonly decimal SHIPPING = 10m;

        public CreateOrderCommandHandler(IOrderRepository orderRepository) 
        {
            _orderRepository = orderRepository;
        }

		public async Task<OrderSummaryDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
			var order = request.Order;

			// Validations
			if (order.Items == null || order.Items.Count == 0)
				throw new ArgumentException("Order must have at least one item.");

			foreach (var item in order.Items)
			{
				if (item.Qty <= 0)
					throw new ArgumentException("Item quantity must be greater than zero");
				if (item.Amount <= 0)
					throw new ArgumentException("Item amount must be greater than zero");
			}

			// Calculate totals
			decimal subTotal = 0;
			foreach (var item in order.Items)
				subTotal += item.Amount;

			decimal tax = subTotal * TAX_RATE;
			decimal total = subTotal + tax + SHIPPING;

			int orderId = await _orderRepository.CreateOrder(order); 

			var summary = new OrderSummaryDTO
			{
				OrderId = orderId,
				Items = order.Items,
				Subtotal = subTotal,
				Tax = tax,
				Shipping = SHIPPING,
				Total = total
			};

			return summary; 
		}
	}
}

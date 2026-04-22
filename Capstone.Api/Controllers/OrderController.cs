using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Capstone.Application.CQRS.Commands;
using Capstone.Application.MediatR;
using Capstone.Core.DTOs;

namespace Capstone.Api.Controllers
{
	[EnableCors(origins: "http://localhost:53019", headers: "*", methods: "*")]
	public class OrderController : ApiController
	{
		private readonly IMessageBus _messageBus;

		public OrderController(IMessageBus messageBus)
		{
			_messageBus = messageBus;
		}

		[HttpPost]
		[Route("api/order")]
		public async Task<IHttpActionResult> CreateOrder([FromBody] CreateOrderDTO order)
		{
			if (order == null || order.Items == null || order.Items.Count == 0)
				return BadRequest("Order must have at least one item");

			var result = await _messageBus.SendAsync(new CreateOrderCommand(order)); 

			return Ok(result);
		}
	}
}

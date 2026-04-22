using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Capstone.Application.CQRS.Queries;
using Capstone.Application.MediatR;
using Capstone.Core.DTOs;
using Capstone.Infrastructure.Repositories;

namespace Capstone.Api.Controllers
{
	[EnableCors(origins: "http://localhost:53019", headers: "*", methods: "*")]
	public class ProductController : ApiController
	{
		private readonly IMessageBus _messageBus;
		public ProductController(IMessageBus messageBus)
		{
			_messageBus = messageBus;
		}

		[HttpGet]
		public async Task<List<ProductDTO>> Get(string search = "", string sortBy = "Name",
	string sortDirection = "asc")
		{
			var products = await _messageBus.SendAsync(new GetProductsQuery 
			{
				Search = search,
				SortBy = sortBy,
				SortDirection = sortDirection
			});

			return products.ToList();
		}

		[HttpGet]
		[Route("api/product/{id}")]
		public async Task<ProductDTO> GetById(int id)
		{
			var product = await _messageBus.SendAsync(new GetProductByIdQuery 
			{ Id = id });

			return product;
		}
	}

}
using Capstone.Application.MediatR;
using System.Web.Http;
using Capstone.Application.CQRS.Queries;
using System.Collections.Generic;
using Capstone.Infrastructure.Repositories;
using System.Web.Http.Cors;

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

        public List<ProductDTO> Get()
        {
            var products = _messageBus.Send(new GetProductsQuery());
            return products;

        }
    }
}
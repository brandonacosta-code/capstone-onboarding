using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Core.DTOs;
using Capstone.Core.Interfaces;
using Capstone.Infrastructure.Repositories;
using MediatR;

namespace Capstone.Application.CQRS.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDTO>>
    { 
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }

    public class GetProductsQueryHandle : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandle(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

		public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var result = await _productRepository.GetProducts(
				request.Search,
				request.SortBy,
				request.SortDirection
			);

			return result.ToList();
		}
	}
}

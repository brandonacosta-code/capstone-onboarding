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

namespace Capstone.Application.CQRS.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDTO>
    {
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productReposiroty)
        { 
            _productRepository = productReposiroty;
        }

		public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
			return await _productRepository.GetProductById(request.Id); 
		}
	}
}

using GreenSlate.Core.KendoDynamicLinq;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Infrastructure.Repositories;
using System.Collections.Generic;

namespace Capstone.Application.CQRS.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDTO>>
    { }

    public class GetProductsQueryHandle : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandle(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_productRepository.GetProducts().AsEnumerable().ToList());
        }
    }
}

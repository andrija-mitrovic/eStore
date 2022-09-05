using Application.Common.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Products.Queries.GetProducts
{
    internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(
            IProductRepository productRepository, 
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();

            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}

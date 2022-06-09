using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetSimilarProductFromOrderQuery : IRequest<List<ProductDto>>
    {
        public int Limit { get; set; } = 10;
    }

    public class
        GetSimilarProductFromOrderQueryHandler : IRequestHandler<GetSimilarProductFromOrderQuery, List<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetSimilarProductFromOrderQueryHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<List<ProductDto>> Handle(GetSimilarProductFromOrderQuery request,
            CancellationToken cancellationToken)
        {
            var order = await GetLatestOrderItemsAsync(cancellationToken);


            if (order.Any())
            {
                var productCategoryIds = order.Select(x => x.Product.ProductCategoryId).ToList();
                var productIds = order.Select(x => x.ProductId).ToList();

                var product = await GetProductsBasedOnOrderListAsync(request,
                    productCategoryIds, productIds, cancellationToken);

                return _mapper.Map<List<ProductDto>>(product);
                
            }

            var products = await _context.Products
                .Include(x => x.CustomerProductPrices)
                .Where(x => x.CustomerProductPrices.Any(c => c.CustomerId == Guid.Parse(_currentCustomerService.Id)))
                .OrderBy(x => x.Name)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ProductDto>>(products);
        }


        #region Service

        private async Task<List<Product>> GetProductsBasedOnOrderListAsync(GetSimilarProductFromOrderQuery request,
            List<long?> productCategoryIds,
            List<long> productIds, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Where(x =>
                    productCategoryIds.Contains(x.ProductCategoryId)
                    && !productIds.Contains(x.Id) && x.CustomerProductPrices
                        .Any(c => c.CustomerId == Guid.Parse(_currentCustomerService.Id)))
                        .Include(x => x.CustomerProductPrices)
                        .Take(request.Limit)
                        .ToListAsync(cancellationToken);
        }

        private async Task<List<OrderItem>> GetLatestOrderItemsAsync(CancellationToken cancellationToken)
        {
            return await _context.OrderItems
                .OrderByDescending(x => x.CreateDate)
                .Where(x => x.CustomerId == Guid.Parse(_currentCustomerService.Id))
                .Include(x => x.Product)
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        #endregion
    }
}
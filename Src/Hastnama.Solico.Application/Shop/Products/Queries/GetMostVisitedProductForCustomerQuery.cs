using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.Statistic;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetMostVisitedProductForCustomerQuery : IRequest<List<ProductDto>>
    {
        public int Limit { get; set; } = 10;
    }

    public class
        GetMostVisitedProductForCustomerQueryHandler : IRequestHandler<GetMostVisitedProductForCustomerQuery,
            List<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetMostVisitedProductForCustomerQueryHandler(ISolicoDbContext context,
            IMapper mapper, ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<List<ProductDto>> Handle(GetMostVisitedProductForCustomerQuery request,
            CancellationToken cancellationToken)
        {
             var mostVisitedProduct = await MostVisitedProductByCustomerAsync(cancellationToken);

            IQueryable<Product> products = _context.Products
                .Where(x=>x.CustomerProductPrices.Any(c=>c.CustomerId==Guid.Parse(_currentCustomerService.Id)))
                .Include(x => x.CustomerProductPrices);;

            if (mostVisitedProduct.Any())
            {
                products = products
                    .Where(x => mostVisitedProduct.Select(m => m.Product.ProductCategoryId)
                        .Contains(x.ProductCategoryId) );
            }

            var productList =await products.Take(request.Limit).ToListAsync(cancellationToken);

            var productDto = _mapper.Map<List<ProductDto>>(productList);
            

            return productDto;
        }


        #region Service
        
        private async Task<List<CustomerProductView>> MostVisitedProductByCustomerAsync(
            CancellationToken cancellationToken)
        {
            return await _context.CustomerProductViews
                .Where(x => x.CustomerId == Guid.Parse(_currentCustomerService.Id))
                .OrderByDescending(x => x.ViewCount)
                .Include(x => x.Product)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
        
        #endregion
    }
}
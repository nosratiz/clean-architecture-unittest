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
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetNewProductForCustomerQuery : IRequest<List<ProductDto>>
    {
        public int limit { get; set; } = 10;
    }

    public class
        GetNewProductForCustomerQueryHandler : IRequestHandler<GetNewProductForCustomerQuery, List<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;


        public GetNewProductForCustomerQueryHandler(ISolicoDbContext context, IMapper mapper, ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<List<ProductDto>> Handle(GetNewProductForCustomerQuery request,
            CancellationToken cancellationToken)
        {
             var products = await GetNewProductsListAsync(request, cancellationToken);

            var productDto = _mapper.Map<List<ProductDto>>(products);

            foreach (var item in productDto)
            {
                var productPrice = await _context.CustomerProductPrices.FirstOrDefaultAsync(x =>
                        x.CustomerId == Guid.Parse(_currentCustomerService.Id) && x.ProductId == item.Id,
                    cancellationToken);

                item.Price = productPrice?.Price ?? 0;
            }

            return productDto;
        }


        #region Service

        private async Task<List<Product>> GetNewProductsListAsync(GetNewProductForCustomerQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Products
                .OrderByDescending(x => x.CreateDate)
                .Where(x=>x.CustomerProductPrices.Any(c=>c.CustomerId==Guid.Parse(_currentCustomerService.Id)))
                .Include(x => x.CustomerProductPrices)
                .Take(request.limit)
                .ToListAsync(cancellationToken);
        }

        #endregion
    }
}
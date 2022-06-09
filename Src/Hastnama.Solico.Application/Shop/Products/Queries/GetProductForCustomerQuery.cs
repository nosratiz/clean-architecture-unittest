using System;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Domain.Models.Statistic;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetProductForCustomerQuery : PagingOptions, IRequest<PagedList<ProductDto>>
    {
        public long? ProductCategoryId { get; set; }
    }

    public class GetProductForCustomerQueryHandler : PagingService<Product>,
        IRequestHandler<GetProductForCustomerQuery, PagedList<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetProductForCustomerQueryHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentCustomerService currentCustomerService, IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<PagedList<ProductDto>> Handle(GetProductForCustomerQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Product> products = _context.Products
                .Where(x => x.CustomerProductPrices.Any(c => c.CustomerId == Guid.Parse(_currentCustomerService.Id)))
                .Include(x => x.CustomerProductPrices);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                products = products.Where(x => x.Name.Contains(request.Search)
                                               || x.Description.Contains(request.Search) ||
                                               x.MaterialId.Contains(request.Search));
            }

            if (request.ProductCategoryId.HasValue)
            {
                var category = await _context.ProductCategories.Include(x => x.Children)
                    .SingleOrDefaultAsync(x => x.Id == request.ProductCategoryId, cancellationToken);

                if (category.Children.Any())
                {
                    var ids = category.Children.Select(x => x.Id).ToList();

                    ids.Add(request.ProductCategoryId.Value);

                    products = products.Where(x => ids.Contains((long)x.ProductCategoryId));
                }
                else
                {
                    products = products.Where(x => x.ProductCategoryId == request.ProductCategoryId);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Sort))
            {
                switch (request.Sort.ToLower())
                {
                    case "price":
                        products = request.Desc
                            ? products.OrderByDescending(x => x.CustomerProductPrices.FirstOrDefault().Price)
                            : products.OrderBy(x => x.CustomerProductPrices.FirstOrDefault().Price);
                        break;
                    case "createdate":
                        products = request.Desc
                            ? products.OrderByDescending(x => x.CreateDate)
                            : products.OrderBy(x => x.CreateDate);
                        break;
                    case "mostvisited":
                        var mostVisitedProduct = await MostVisitedProductByCustomerAsync(cancellationToken);

                        if (mostVisitedProduct.Any())
                        {
                            products = products
                                .Where(x => mostVisitedProduct.Select(m => m.Product.ProductCategoryId)
                                                .Contains(x.ProductCategoryId) &&
                                            !mostVisitedProduct.Select(m => m.ProductId).Contains(x.Id));
                        }

                        break;

                    case "similarOrder":
                        var order = await GetLatestOrderItemsAsync(cancellationToken);


                        if (order.Any())
                        {
                            var productCategoryIds = order.Select(x => x.Product.ProductCategoryId).ToList();
                            var productIds = order.Select(x => x.ProductId).ToList();

                            products = products.Where(x =>
                                    productCategoryIds.Contains(x.ProductCategoryId)
                                    && !productIds.Contains(x.Id) && x.CustomerProductPrices
                                        .Any(c => c.CustomerId == Guid.Parse(_currentCustomerService.Id)));

                        }
                        break;
                    default:
                        products = products.OrderBy(x => x.Name);
                        break;
                }
            }

            var productList = await GetPagedAsync(request.Page, request.Limit, products, cancellationToken);

            var productDto = productList.MapTo<ProductDto>(_mapper);


            return productDto;
        }


        private async Task<List<CustomerProductView>> MostVisitedProductByCustomerAsync(
            CancellationToken cancellationToken)
        {
            return await _context.CustomerProductViews
                .Where(x => x.CustomerId == Guid.Parse(_currentCustomerService.Id))
                .OrderByDescending(x => x.ViewCount)
                .Include(x => x.Product)
                .Take(20)
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


        #region Service

        #endregion
    }
}
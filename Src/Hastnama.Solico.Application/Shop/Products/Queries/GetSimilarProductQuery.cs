using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetSimilarProductQuery : IRequest<Result<List<ProductDto>>>
    {
        public long Id { get; set; }

        public int limit { get; set; }
    }

    public class GetSimilarProductQueryHandler : IRequestHandler<GetSimilarProductQuery, Result<List<ProductDto>>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetSimilarProductQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<Result<List<ProductDto>>> Handle(GetSimilarProductQuery request,
            CancellationToken cancellationToken)
        {
            var product = await GetProductAsync(request, cancellationToken);

            if (product is null)
            {
                return Result<List<ProductDto>>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ProductNotFound,
                        cancellationToken))));
            }

            var products = await GetSimilarProductListAsync(request, cancellationToken, product,
                Guid.Parse(_currentCustomerService.Id));


            return Result<List<ProductDto>>
                .SuccessFul(_mapper.Map<List<ProductDto>>(products));
        }


        #region Query

        private async Task<List<Product>> GetSimilarProductListAsync(GetSimilarProductQuery request,
            CancellationToken cancellationToken, Product product, Guid customerId)
        {
            return await _context.Products
                .Include(x => x.CustomerProductPrices)
                .Where(x => x.ProductCategoryId == product.ProductCategoryId && x.Id != product.Id &&
                            x.CustomerProductPrices.Any(c => c.CustomerId == customerId))
                .Take(request.limit)
                .ToListAsync(cancellationToken);
        }

        private async Task<Product> GetProductAsync(GetSimilarProductQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
    }
}
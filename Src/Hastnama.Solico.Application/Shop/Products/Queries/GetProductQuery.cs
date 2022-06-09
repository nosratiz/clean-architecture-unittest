using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Interfaces.Statistic;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetProductQuery : IRequest<Result<ProductDto>>
    {
        public long Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly ICustomerProductViewService _customerProductViewService;


        public GetProductQueryHandler(ISolicoDbContext context, ILocalization localization, IMapper mapper
            , ICurrentCustomerService currentCustomerService,
            ICustomerProductViewService customerProductViewService)
        {
            _context = context;
            _localization = localization;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
            _customerProductViewService = customerProductViewService;
        }

        public async Task<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await GetProductAsync(request, cancellationToken);

            if (product is null)
            {
                return Result<ProductDto>.Failed(new NotFoundObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ProductNotFound,
                        cancellationToken))));
            }

            try
            {
                await _customerProductViewService.UpdateViewCount(Guid.Parse(_currentCustomerService.Id), product.Id,
                    cancellationToken);
            }
            catch (Exception e)
            {
                Log.Information(e.Message, e.Source);
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Result<ProductDto>.SuccessFul(productDto);
        }

        #region Service

        private async Task<Product> GetProductAsync(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(x => x.ProductCategory)
                .ThenInclude(x=>x.Children)
                .ThenInclude(x=>x.Children)
                .Include(x => x.ProductGalleries)
                .Include(x => x.CustomerProductPrices)
                .Include(x=>x.ProductMedias)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
    }
}
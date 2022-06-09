using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces.Statistic;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Microsoft.Extensions.Caching.Distributed;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<List<AvailableProductDto>> AvailableProductForCustomer(CancellationToken cancellationToken);

        List<ProductDto>  SetProductPrice( List<ProductDto> productDto, List<AvailableProductDto> availableProductForCustomer);
    }
    
    public class  ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly ISolicoWebService _solicoWebService;
        private readonly ICustomerProductViewService _customerProductViewService;

        public ProductService(IMapper mapper, IDistributedCache cache, ICurrentCustomerService currentCustomerService, ISolicoWebService solicoWebService, ICustomerProductViewService customerProductViewService)
        {
            _mapper = mapper;
            _cache = cache;
            _currentCustomerService = currentCustomerService;
            _solicoWebService = solicoWebService;
            _customerProductViewService = customerProductViewService;
        }

        public async Task<List<AvailableProductDto>> AvailableProductForCustomer(CancellationToken cancellationToken)
        {
            var availableProductForCustomer =
                await _cache.GetRecordAsync<List<AvailableProductDto>>(
                    $"Customer-{_currentCustomerService.CustomerId}",
                    cancellationToken);

            if (availableProductForCustomer is null)
            {
                var ProductForCustomer = await
                    _solicoWebService.GetPriceServiceAsync(
                        new List<SolicoPricingDto> { new() { KUNNR = _currentCustomerService.CustomerId } },
                        cancellationToken);

                availableProductForCustomer = ProductForCustomer
                    .Select(x => new AvailableProductDto { MATNR = x.MATNR, GROSS_PRICE = x.GROSS_PRICE }).ToList();

                await _cache.SetRecordAsync($"Customer-{_currentCustomerService.CustomerId}",
                    availableProductForCustomer, cancellationToken, TimeSpan.FromMinutes(5));
            }

            return availableProductForCustomer;
        }

        public List<ProductDto> SetProductPrice(List<ProductDto> productDto, List<AvailableProductDto> availableProductForCustomer)
        {
            productDto.ForEach(product =>
            {
                product.Price = availableProductForCustomer
                    .FirstOrDefault(x => x.MATNR == product.MaterialId)
                    !.GROSS_PRICE;
            });

            return productDto;
        }
    }
}
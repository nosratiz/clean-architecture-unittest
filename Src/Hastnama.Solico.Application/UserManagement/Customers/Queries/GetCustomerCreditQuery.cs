using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Hastnama.Solico.Application.UserManagement.Customers.Queries
{
    public class GetCustomerCreditQuery : IRequest<Result<CustomerCreditDto>>
    {
    }

    public class GetCustomerCreditQueryHandler : IRequestHandler<GetCustomerCreditQuery, Result<CustomerCreditDto>>
    {
        private readonly IMapper _mapper;
        private readonly ISolicoWebService _solicoWebService;
        private readonly IDistributedCache _cache;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetCustomerCreditQueryHandler(IMapper mapper,
            ISolicoWebService solicoWebService, IDistributedCache cache,
            ICurrentCustomerService currentCustomerService)
        {
            _mapper = mapper;
            _solicoWebService = solicoWebService;
            _cache = cache;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<Result<CustomerCreditDto>> Handle(GetCustomerCreditQuery request,
            CancellationToken cancellationToken)
        {
            var credit =
                await _cache.GetRecordAsync<CreditDto>($"Credit-{_currentCustomerService.CustomerId}",
                    cancellationToken);

            if (credit is null)
            {
                credit = await _solicoWebService.GetCreditServiceAsync(
                    new List<SolicoPricingDto> {new() {KUNNR = _currentCustomerService.CustomerId}},
                    cancellationToken);

                await _cache.SetRecordAsync($"Credit-{_currentCustomerService.CustomerId}", credit, cancellationToken,
                    TimeSpan.FromMinutes(2));
            }

            return Result<CustomerCreditDto>.SuccessFul(
                _mapper.Map<CustomerCreditDto>(credit.IT_CREDIT_EXPOSURE.FirstOrDefault() ));
        }
    }
}
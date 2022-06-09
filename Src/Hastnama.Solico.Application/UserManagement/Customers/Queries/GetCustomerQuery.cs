using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeviceDetectorNET;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Hastnama.Solico.Application.UserManagement.Customers.Queries
{
    public class GetCustomerQuery : IRequest<Result<CustomerDto>>
    {
        public string SolicoCustomerId { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Result<CustomerDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IMapper _mapper;
        private readonly ISolicoWebService _solicoWebService;

        public GetCustomerQueryHandler(ISolicoDbContext context, ILocalization localization, IMapper mapper,
            ISolicoWebService solicoWebService)
        {
            _context = context;
            _localization = localization;
            _mapper = mapper;
            _solicoWebService = solicoWebService;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var solicoCustomer = await _solicoWebService.GetCustomerAsync(
                    new List<SolicoPricingDto> { new() { KUNNR = request.SolicoCustomerId } },
                    cancellationToken);

                if (IEnumerableExtensions.Any(solicoCustomer) == false)
                    return Result<CustomerDto>.Failed(new BadRequestObjectResult(
                        new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound,
                            cancellationToken))));

                var customer = _mapper.Map<Customer>(solicoCustomer.FirstOrDefault());

                return Result<CustomerDto>.SuccessFul(_mapper.Map<CustomerDto>(customer));
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
                return Result<CustomerDto>.Failed(
                    new BadRequestObjectResult(
                        new ApiMessage(await _localization.GetMessage(ResponseMessage.UnHandledError, cancellationToken))));
            }
        }
    }
}
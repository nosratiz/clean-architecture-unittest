using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace Hastnama.Solico.Application.Shop.Orders.Queries
{
    public class GetSailedOrderQuery : IRequest<Result<SailedOrderDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetSailedOrderQueryHandler : IRequestHandler<GetSailedOrderQuery, Result<SailedOrderDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISolicoWebService _solicoWebService;
        private readonly IDistributedCache _cache;
        private readonly ILocalization _localization;

        public GetSailedOrderQueryHandler(ISolicoDbContext context, IMapper mapper, ISolicoWebService solicoWebService,
            IDistributedCache cache, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _solicoWebService = solicoWebService;
            _cache = cache;
            _localization = localization;
        }

        public async Task<Result<SailedOrderDto>> Handle(GetSailedOrderQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (order is null)
            {
                return Result<SailedOrderDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderNotFound, cancellationToken))));
            }

            if (string.IsNullOrWhiteSpace(order.QuotationNumber))
            {
                return Result<SailedOrderDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization
                    .GetMessage(ResponseMessage.QuotationNumberIsEmpty, cancellationToken))));
            }

            try
            {
                var sailedOrder = await _solicoWebService.GetQuotationSalesOrderServiceAsync(
                    new List<SolicoOrderNumberDto> { new() { VBELN = order.QuotationNumber } },
                    cancellationToken);

                if (sailedOrder.Any() == false)
                {
                    return Result<SailedOrderDto>.Failed(new BadRequestObjectResult(
                        new ApiMessage(await _localization.GetMessage(ResponseMessage.SailedOrderNotCreateInSolico,
                            cancellationToken))));
                }

                return Result<SailedOrderDto>.SuccessFul(
                    _mapper.Map<SailedOrderDto>(sailedOrder.FirstOrDefault()!.ORDER.FirstOrDefault()));
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
                return Result<SailedOrderDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UnHandledError, cancellationToken))));
            }
        }
    }
}
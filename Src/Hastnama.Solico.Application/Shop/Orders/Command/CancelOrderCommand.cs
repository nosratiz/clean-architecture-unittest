using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Orders.Command
{
    public class CancelOrderCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public CancelOrderCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (order is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderNotFound, cancellationToken))));
            }

            if (order.OrderStatus > (int) OrderStatus.Pending)
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.CancelOrderNotAllowed, cancellationToken))));
            }


            order.OrderStatus = (int) OrderStatus.Canceled;

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}
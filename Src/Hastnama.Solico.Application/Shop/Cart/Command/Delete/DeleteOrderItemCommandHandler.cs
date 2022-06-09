using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Cart.Command.Delete
{
    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteOrderItemCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var orderItem = await GetOrderItemAsync(request, cancellationToken);

            if (orderItem is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ItemNotFound, cancellationToken))));
            }

            _context.OrderItems.Remove(orderItem);

            await _context.SaveAsync(cancellationToken);
            
            return Result.SuccessFul();
        }

        #region Query

        private async Task<OrderItem> GetOrderItemAsync(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            return await _context.OrderItems
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
       
    }
}
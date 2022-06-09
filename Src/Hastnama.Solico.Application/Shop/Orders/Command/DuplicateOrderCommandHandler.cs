using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Orders.Command
{
    public class DuplicateOrderCommandHandler : IRequestHandler<DuplicateOrderCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly ICurrentCustomerService _currentCustomer;

        public DuplicateOrderCommandHandler(ISolicoDbContext context, ILocalization localization,
            ICurrentCustomerService currentCustomer)
        {
            _context = context;
            _localization = localization;
            _currentCustomer = currentCustomer;
        }

        public async Task<Result> Handle(DuplicateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await GetOrderAsync(request, cancellationToken);

            if (order is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderNotFound, cancellationToken))));
            }

            foreach (var item in order.OrderItems)
            {
                var orderItem =
                    await _context.OrderItems.FirstOrDefaultAsync(x => x.OrderId==null
                                                                       && x.CustomerId ==Guid.Parse(_currentCustomer.Id)&&
                                                                       x.ProductId == item.ProductId,
                        cancellationToken);
                if (orderItem != null)
                {
                    orderItem.Count += item.Count;
                }
                else
                {
                    await _context.OrderItems.AddAsync(new OrderItem
                    {
                        CustomerId = item.CustomerId,
                        CreateDate = DateTime.Now,
                        Count = item.Count,
                        Price = item.Price,
                        ProductId = item.ProductId,
                    }, cancellationToken);
                }
            }


            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<Order> GetOrderAsync(DuplicateOrderCommand request, CancellationToken cancellationToken)
        {
            return await _context.Orders.Include(x => x.OrderItems)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
    }
}
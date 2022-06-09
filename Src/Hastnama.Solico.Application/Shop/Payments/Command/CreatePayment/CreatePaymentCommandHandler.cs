using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Payments.Command.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<string>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IBankTransactionService _bankTransactionService;

        public CreatePaymentCommandHandler(ISolicoDbContext context, ILocalization localization,
            ICurrentCustomerService currentCustomerService, IBankTransactionService bankTransactionService)
        {
            _context = context;
            _localization = localization;
            _currentCustomerService = currentCustomerService;
            _bankTransactionService = bankTransactionService;
        }

        public async Task<Result<string>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await GetOrderAsync(request, cancellationToken);

            if (order is null)
            {
                return Result<string>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderNotFound, cancellationToken))));
            }

            if (order.OrderStatus == (int) OrderStatus.Posted)
            {
                return Result<string>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderAlreadyPaid,
                        cancellationToken))));
            }

            var result = await _bankTransactionService.Purchase(order, cancellationToken);

            if (result.Success == false)
            {
                return Result<string>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));
            }

            return Result<string>.SuccessFul(result.Data);
        }

        #region Query

        private async Task<Order> GetOrderAsync(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            return await _context.Orders.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
       
    }
}
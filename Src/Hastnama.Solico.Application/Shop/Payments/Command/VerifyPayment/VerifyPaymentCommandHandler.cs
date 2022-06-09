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

namespace Hastnama.Solico.Application.Shop.Payments.Command.VerifyPayment
{
    public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IBankTransactionService _bankTransactionService;


        public VerifyPaymentCommandHandler(ISolicoDbContext context,
            ILocalization localization, IBankTransactionService bankTransactionService)
        {
            _context = context;
            _localization = localization;
            _bankTransactionService = bankTransactionService;
        }


        public async Task<Result> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        {
            var bankTransaction = await GetBankTransactionAsync(request, cancellationToken);

            if (bankTransaction is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderNotFound, cancellationToken))));
            }

            var result = await _bankTransactionService.VerifyPayment(request.Token, bankTransaction.Order.OrderIndex,
                Convert.ToInt64(bankTransaction.Order.FinalAmount), cancellationToken);

            if (result.Success == false)
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));
            }

            bankTransaction.Order.IsPaid = true;
            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<BankTransaction> GetBankTransactionAsync(VerifyPaymentCommand request,
            CancellationToken cancellationToken)
        {
            return await _context.BankTransactions
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Token == request.Token, cancellationToken);
        }

        #endregion
    }
}
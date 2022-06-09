using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Common.Sms;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode
{
    public class SendConfirmCodeCommandHandler : IRequestHandler<SendConfirmCodeCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IPayamakService _payamakService;
        private readonly IKaveNegarService _kaveNegarService;

        public SendConfirmCodeCommandHandler(ISolicoDbContext context, ILocalization localization,
            IPayamakService payamakService, IKaveNegarService kaveNegarService)
        {
            _context = context;
            _localization = localization;
            _payamakService = payamakService;
            _kaveNegarService = kaveNegarService;
        }

        public async Task<Result> Handle(SendConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerAsync(request, cancellationToken);


            if (customer is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));
            }

            customer.ActiveCode = new Random().Next(10000, 99999).ToString();
            customer.ExpiredVerification = DateTime.Now.AddMinutes(2);

            await _context.SaveAsync(cancellationToken);

            try
            {
                // await _payamakService.SendMessage(customer.Mobile, customer.ActiveCode);
                await _kaveNegarService.SendConfirmedCode(customer.Mobile, customer.ActiveCode);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
                return Result.Failed(
                    new BadRequestObjectResult(
                        new ApiMessage(await _localization.GetMessage(ResponseMessage.SmsNotSend,cancellationToken))));
            }

            return Result.SuccessFul();
        }

        #region Query

        private async Task<Customer> GetCustomerAsync(SendConfirmCodeCommand request,
            CancellationToken cancellationToken)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Mobile == request.Mobile, cancellationToken);
        }

        #endregion
    }
}
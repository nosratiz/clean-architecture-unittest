using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.ChangePassword
{
    public class ChangePasswordCustomerCommandHandler : IRequestHandler<ChangePasswordCustomerCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly ICurrentCustomerService _currentCustomerService;

        public ChangePasswordCustomerCommandHandler(ISolicoDbContext context, ILocalization localization,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _localization = localization;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<Result> Handle(ChangePasswordCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer =
                await _context.Customers
                    .SingleOrDefaultAsync(x => x.Id == Guid.Parse(_currentCustomerService.Id), cancellationToken);

            if (PasswordManagement.CheckPassword(request.Password, customer.Password) == false)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.WrongPassword, cancellationToken))));
            }

            if (request.Password == request.NewPassword)
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.PasswordRecentlyUsed, cancellationToken))));
            }

            customer.Password = PasswordManagement.HashPass(request.NewPassword);
            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}
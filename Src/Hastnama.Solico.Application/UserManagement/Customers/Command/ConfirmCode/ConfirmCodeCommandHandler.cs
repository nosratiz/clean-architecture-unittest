using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode
{
    public class ConfirmCodeCommandHandler : IRequestHandler<ConfirmCodeCommand, Result<AuthResult>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly ITokenGenerator _tokenGenerator;

        public ConfirmCodeCommandHandler(ISolicoDbContext context, ILocalization localization,
            ITokenGenerator tokenGenerator)
        {
            _context = context;
            _localization = localization;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<AuthResult>> Handle(ConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerAsync(request, cancellationToken);

            if (customer is null)
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));
            }

            if (customer.ActiveCode != request.Code)
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.WrongCode, cancellationToken))));
            }

            if (customer.ExpiredVerification!.Value <= DateTime.Now)
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ActiveCodeExpired,
                        cancellationToken))));
            }

            var token = await _tokenGenerator.GenerateCustomerToken(customer, cancellationToken);

            return Result<AuthResult>.SuccessFul(token);
        }

        #region Query

        private async Task<Customer> GetCustomerAsync(ConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Mobile == request.Mobile, cancellationToken);
        }

        #endregion
      
    }
}
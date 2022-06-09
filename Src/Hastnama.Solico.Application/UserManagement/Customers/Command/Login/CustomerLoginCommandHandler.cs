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

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.Login
{
    public class CustomerLoginCommandHandler : IRequestHandler<CustomerLoginCommand, Result<AuthResult>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly ITokenGenerator _tokenGenerator;

        public CustomerLoginCommandHandler(ISolicoDbContext context, ILocalization localization,
            ITokenGenerator tokenGenerator)
        {
            _context = context;
            _localization = localization;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<AuthResult>> Handle(CustomerLoginCommand request, CancellationToken cancellationToken)
        {
            var Customer = await GetCustomerAsync(request, cancellationToken);

            if (Customer is null)
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidUserOrPassword,
                        cancellationToken))));
            }


            if (string.IsNullOrWhiteSpace(Customer.Password))
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.PasswordNotSet, cancellationToken))));
            }

            if (PasswordManagement.CheckPassword(request.Passwords, Customer.Password) == false)
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidUserOrPassword,
                        cancellationToken))));
            }

            if (Customer.IsActive == false)
            {
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.CustomerIsDeactivate, cancellationToken))));
            }


            var token = await _tokenGenerator.GenerateCustomerToken(Customer, cancellationToken);

            return Result<AuthResult>.SuccessFul(token);
        }

        #region Query

        private async Task<Customer> GetCustomerAsync(CustomerLoginCommand request, CancellationToken cancellationToken)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Mobile == request.Mobile, cancellationToken);
        }

        #endregion
    }
}
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public UpdateCustomerCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await GetCustomerAsync(request, cancellationToken);

            if (customer is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));
            }

            customer.Mobile = request.Mobile;
            customer.Password = PasswordManagement.HashPass(request.Password);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<Customer> GetCustomerAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _context.Customers.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
       
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.Activation
{
    public class ActivationCustomerCommandHandler : IRequestHandler<ActivationCustomerCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public ActivationCustomerCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(ActivationCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));
            }

            customer.IsActive = !customer.IsActive;

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}
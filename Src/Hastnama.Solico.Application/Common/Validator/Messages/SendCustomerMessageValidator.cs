using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Messages
{
    public class SendCustomerMessageValidator : AbstractValidator<SendCustomerMessageCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        public SendCustomerMessageValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;
            RuleFor(dto => dto.Content).NotEmpty().NotNull();

            RuleFor(dto => dto.Title).NotEmpty().NotNull();

            RuleFor(dto => dto.CustomerId).MustAsync(ValidCustomer)
                .WithMessage(_localization.GetMessage(ResponseMessage.UserNotFound));
        }

        private async Task<bool> ValidCustomer(Guid customerId, CancellationToken cancellationToken)
        {
            if (await _context.Customers.AnyAsync(x=>x.Id==customerId,cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}
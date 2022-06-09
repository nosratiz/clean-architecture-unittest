using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Cms.Subscribers.Command.Create;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Subscribers
{
    public class CreateSubscriberCommandValidator : AbstractValidator<CreateSubscriberCommand>
    {
        private readonly ILocalization _localization;
        private readonly ISolicoDbContext _context;

        public CreateSubscriberCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Email)
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
                .EmailAddress()
                .MustAsync(CheckDuplicateEmail)
                .WithMessage(_localization.GetMessage(ResponseMessage.EmailIsAlreadyExist));
        }

        private async Task<bool> CheckDuplicateEmail(string email, CancellationToken cancellationToken)
        {
            if (await _context.Subscribes.AnyAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken))
            {
                return false;
            }

            return true;
        }
    }
}
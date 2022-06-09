using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Customers
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public UpdateCustomerCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired))
                .MinimumLength(6);

            RuleFor(dto => dto.Mobile)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired));

            RuleFor(dto => dto).MustAsync(ValidMobile)
                .WithMessage(_localization.GetMessage(ResponseMessage.MobileAlreadyExist));
        }

        private async Task<bool> ValidMobile(UpdateCustomerCommand updateCustomerCommand,
            CancellationToken cancellationToken)
        {
            return !await _context.Customers.AnyAsync(
                x => x.Mobile == updateCustomerCommand.Mobile && x.Id != updateCustomerCommand.Id, cancellationToken);
        }
    }
}
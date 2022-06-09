using FluentValidation;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ChangePassword;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;

namespace Hastnama.Solico.Application.Common.Validator.Customers
{
    public class ChangePasswordCustomerCommandValidator : AbstractValidator<ChangePasswordCustomerCommand>
    {
        private readonly ILocalization _localization;

        public ChangePasswordCustomerCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired));

            RuleFor(dto => dto.NewPassword)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired))
                .MinimumLength(6);
        }
    }
}
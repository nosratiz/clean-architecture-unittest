using FluentValidation;
using Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;

namespace Hastnama.Solico.Application.Common.Validator.Customers
{
    public class SendConfirmCodeCommandValidator : AbstractValidator<SendConfirmCodeCommand>
    {
        private readonly ILocalization _localization;
        public SendConfirmCodeCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;
           
            RuleFor(dto => dto.Mobile)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired));
        }
    }
}
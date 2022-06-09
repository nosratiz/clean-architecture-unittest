using FluentValidation;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode;

namespace Hastnama.Solico.Application.Common.Validator.Customers
{
    public class ConfirmCodeCommandValidator : AbstractValidator<ConfirmCodeCommand>
    {
        public ConfirmCodeCommandValidator()
        {
            RuleFor(dto => dto.Code).NotEmpty().NotNull();

            RuleFor(dto => dto.Mobile).NotEmpty().NotNull();
        }
    }
}
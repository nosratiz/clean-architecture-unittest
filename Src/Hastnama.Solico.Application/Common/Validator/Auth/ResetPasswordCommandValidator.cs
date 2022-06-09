using FluentValidation;
using Hastnama.Solico.Application.Auth.Command.ResetPassword;

namespace Hastnama.Solico.Application.Common.Validator.Auth
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(dto => dto.ActiveCode).NotEmpty().NotNull();

            RuleFor(dto => dto.Email).NotEmpty().NotNull();

            RuleFor(dto => dto.NewPassword).NotEmpty().NotNull();
        }
    }
}
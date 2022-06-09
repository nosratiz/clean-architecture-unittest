using FluentValidation;
using Hastnama.Solico.Application.Auth.Command.Login;

namespace Hastnama.Solico.Application.Common.Validator.Auth
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(dto => dto.Password).NotEmpty().NotNull();

            RuleFor(dto => dto.UserName).NotEmpty().NotNull();
        }
    }
}
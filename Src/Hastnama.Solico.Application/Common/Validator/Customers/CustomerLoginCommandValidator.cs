using FluentValidation;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Login;

namespace Hastnama.Solico.Application.Common.Validator.Customers
{
    public class CustomerLoginCommandValidator : AbstractValidator<CustomerLoginCommand>
    {
        public CustomerLoginCommandValidator()
        {
            RuleFor(dto => dto.Mobile).NotEmpty().NotNull();

            RuleFor(dto => dto.Passwords).NotEmpty().NotNull();
        }
    }
}
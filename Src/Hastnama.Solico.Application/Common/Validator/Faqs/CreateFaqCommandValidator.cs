using FluentValidation;
using Hastnama.Solico.Application.Cms.Faqs.Command.Create;

namespace Hastnama.Solico.Application.Common.Validator.Faqs
{
    public class CreateFaqCommandValidator : AbstractValidator<CreateFaqCommand>
    {
        public CreateFaqCommandValidator()
        {
            RuleFor(dto => dto.Answer).NotEmpty().NotNull();

            RuleFor(dto => dto.Question).NotEmpty().NotNull();
        }
    }
}
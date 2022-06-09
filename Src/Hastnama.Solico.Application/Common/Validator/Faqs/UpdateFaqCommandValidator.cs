using FluentValidation;
using Hastnama.Solico.Application.Cms.Faqs.Command.Update;

namespace Hastnama.Solico.Application.Common.Validator.Faqs
{
    public class UpdateFaqCommandValidator : AbstractValidator<UpdateFaqCommand>
    {
        public UpdateFaqCommandValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty().NotNull();
          
            RuleFor(dto => dto.Answer).NotEmpty().NotNull();

            RuleFor(dto => dto.Question).NotEmpty().NotNull();

        }
    }
}
using FluentValidation;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Create;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;

namespace Hastnama.Solico.Application.Common.Validator.ContactUses
{
    public class CreateContactUsCommandValidator : AbstractValidator<CreateContactUsCommand>
    {
        private readonly ILocalization _localization;

        public CreateContactUsCommandValidator(ILocalization localization)
        {
            _localization = localization;
            
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));


            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
                .EmailAddress().WithMessage(_localization.GetMessage(ResponseMessage.InvalidEmailFormat));


            RuleFor(dto => dto.Message)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.MessageIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.MessageIsRequired));
        }
    }
}
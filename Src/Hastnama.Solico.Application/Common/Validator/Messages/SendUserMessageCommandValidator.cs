using FluentValidation;
using Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage;

namespace Hastnama.Solico.Application.Common.Validator.Messages
{
    public class SendUserMessageCommandValidator : AbstractValidator<SendUserMessageCommand>
    {
        public SendUserMessageCommandValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty().NotNull();

            RuleFor(dto => dto.Content).NotEmpty().NotNull();
        }
    }
}
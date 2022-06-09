using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyCustomerMessage;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Messages
{
    public class ReplyCustomerCommandValidator : AbstractValidator<ReplyCustomerMessageCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public ReplyCustomerCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;
            RuleFor(dto => dto.Content)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.ContentIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.ContentIsRequired));

            RuleFor(dto => dto.ParentMessageId).MustAsync(ValidMessage)
                .WithMessage(_localization.GetMessage(ResponseMessage.MessageNotFound));
        }

        private async Task<bool> ValidMessage(Guid messageId, CancellationToken cancellationToken)
        {
            if (await _context.messages.AnyAsync(x => x.Id == messageId, cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}
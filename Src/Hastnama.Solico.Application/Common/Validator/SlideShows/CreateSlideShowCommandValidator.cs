using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Create;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.SlideShows
{
    public class CreateSlideShowCommandValidator : AbstractValidator<CreateSlidShowCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public CreateSlideShowCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Image)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.ImageIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.ImageIsRequired));

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.SortOrder)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SortOrderIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SortOrderIsRequired))
            .MustAsync(ValidSortOrder).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateSortOrder));

            RuleFor(dto => dto)
            .Must(ValidDateTime).WithMessage(_localization.GetMessage(ResponseMessage.InvalidDateTimePeriod));
        }

        private async Task<bool> ValidSortOrder(int sortOrder, CancellationToken cancellation)
        {
            if (await _context.SlideShows.AnyAsync(x => x.SortOrder == sortOrder, cancellation))
                return false;

            return true;

        }

        private bool ValidDateTime(CreateSlidShowCommand createSlidShow)
        {

            if (createSlidShow.StartDateTime.HasValue && createSlidShow.EndDateTime.HasValue)
                if (createSlidShow.StartDateTime > createSlidShow.EndDateTime)
                    return false;

            return true;

        }


    }
}
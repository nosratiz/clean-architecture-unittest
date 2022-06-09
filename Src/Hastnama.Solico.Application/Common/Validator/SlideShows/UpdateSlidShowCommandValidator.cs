using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Update;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.SlideShows
{
    public class UpdateSlidShowCommandValidator : AbstractValidator<UpdateSlideShowCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public UpdateSlidShowCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;

            CascadeMode = CascadeMode.Stop;


            RuleFor(dto => dto.Image)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.ImageIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.ImageIsRequired));

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.SortOrder)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SortOrderIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SortOrderIsRequired));

            RuleFor(dto => dto)
            .Must(ValidDateTime).WithMessage(_localization.GetMessage(ResponseMessage.InvalidDateTimePeriod))
            .MustAsync(ValidSortOrder).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateSortOrder));

        }

        private async Task<bool> ValidSortOrder(UpdateSlideShowCommand updateslideShod, CancellationToken cancellation)
        {
            if (await _context.SlideShows.AnyAsync(x => x.SortOrder == updateslideShod.SortOrder && x.Id != updateslideShod.Id, cancellation))
                return false;

            return true;

        }


        private bool ValidDateTime(UpdateSlideShowCommand updateslideShod)
        {

            if (updateslideShod.StartDateTime.HasValue && updateslideShod.EndDateTime.HasValue)
                if (updateslideShod.StartDateTime > updateslideShod.EndDateTime)
                    return false;

            return true;

        }

    }
}
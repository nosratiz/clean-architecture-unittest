using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Companies.Command.Update;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Companies
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        public UpdateCompanyCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;

            RuleFor(dto => dto.Id).NotEmpty().NotNull();

            RuleFor(dto => dto.Name)
          .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
          .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.Slug)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SlugIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SlugIsRequired));

            RuleFor(dto => dto)
            .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateName))
            .MustAsync(ValidSlug).WithMessage(_localization.GetMessage(ResponseMessage.SlugIsExist));
        }

        private async Task<bool> ValidName(UpdateCompanyCommand updateCompanyCommand, CancellationToken cancellation)
        {
            if (await _context.Companies.AnyAsync(x => x.Id != updateCompanyCommand.Id && x.Name == updateCompanyCommand.Name, cancellation))
                return false;

            return true;

        }

        private async Task<bool> ValidSlug(UpdateCompanyCommand updateCompanyCommand, CancellationToken cancellation)
        {
            if (await _context.Companies.AnyAsync(x => x.Id != updateCompanyCommand.Id && x.Slug == updateCompanyCommand.Slug, cancellation))
                return false;

            return true;
        }
    }
}
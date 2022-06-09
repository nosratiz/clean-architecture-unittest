using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Companies.Command.Create;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Companies
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        public CreateCompanyCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateName));

            RuleFor(dto=>dto.Slug)
            .MustAsync(ValidSlug)
            .WithMessage(_localization.GetMessage(ResponseMessage.SlugIsExist));

        }

        private async Task<bool> ValidSlug(string slug, CancellationToken cancellation)
        {

            if (!string.IsNullOrWhiteSpace(slug))
                if (await _context.Companies.AnyAsync(x => x.Slug == slug, cancellation))
                    return false;

            return true;
        }

        private async Task<bool> ValidName(string name, CancellationToken cancellation)
        {
            if (await _context.Companies.AnyAsync(x => x.Name == name, cancellation))
                return false;

            return true;
        }
    }
}
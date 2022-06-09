using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Create;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.ProductCategories
{
    public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
    {
        private readonly ILocalization _localization;
        private readonly ISolicoDbContext _context;
        public CreateProductCategoryCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;

            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateName));


            RuleForEach(dto => dto.ChildrenNames)
                .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateName));


        }

        private async Task<bool> ValidName(string name, CancellationToken cancellationToken)
        {
            if (await _context.ProductCategories.AnyAsync(x=>x.Name==name,cancellationToken))
            {
                return false;
            }

            return true;
        }
    }
}
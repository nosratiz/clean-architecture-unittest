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
    public class CreateCChildrenCategoryCommandValidator : AbstractValidator<CreateChildrenCategoryCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public CreateCChildrenCategoryCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
            CascadeMode = CascadeMode.Stop;


            RuleFor(dto => dto.Name)
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateName));


            RuleFor(dto => dto.ParentId).MustAsync(ValidParent)
                .WithMessage(_localization.GetMessage(ResponseMessage.CategoryNotFound));
        }

        private async Task<bool> ValidName(string name, CancellationToken cancellationToken)
        {
            if (await _context.ProductCategories.AnyAsync(x => x.Name == name, cancellationToken))
            {
                return false;
            }

            return true;
        }


        private async Task<bool> ValidParent(int parentId, CancellationToken cancellationToken)
        {
            if (await _context.ProductCategories.AnyAsync(x => x.Id == parentId, cancellationToken))
            {
                return true;
            }

            return false;
        }
    }
}
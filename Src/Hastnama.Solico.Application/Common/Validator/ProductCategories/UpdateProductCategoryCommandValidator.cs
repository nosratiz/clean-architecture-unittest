using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Update;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.ProductCategories
{

    public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public UpdateProductCategoryCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;

            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Id).NotEmpty().NotNull();

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));


            RuleFor(dto => dto.Slug)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SlugIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SlugIsRequired));

            RuleFor(dto => dto.ParentId)
            .MustAsync(ValidParent)
            .WithMessage(_localization.GetMessage(ResponseMessage.CategoryNotFound));


            RuleFor(dto => dto).MustAsync(ValidSlug).WithMessage(_localization.GetMessage(ResponseMessage.SlugIsExist))
                .MustAsync(ValidName).WithMessage(_localization.GetMessage(ResponseMessage.DuplicateName));
            
        }


        private async Task<bool> ValidParent(int? parentId, CancellationToken cancellation)
        {

            if (parentId.HasValue)
                if (!await _context.ProductCategories.AnyAsync(x => x.Id == parentId, cancellation))
                    return false;
            return true;
        }
        

        private async Task<bool> ValidSlug(UpdateProductCategoryCommand updateProductCategoryCommand, CancellationToken cancellation)
        {
            if (await _context.ProductCategories.AnyAsync(x => x.Slug == updateProductCategoryCommand.Slug && x.Id != updateProductCategoryCommand.Id, cancellation))
                return false;

            return true;
        }

        private async Task<bool> ValidName(UpdateProductCategoryCommand updateProductCategoryCommand,
            CancellationToken cancellationToken)
        {
            if (await _context.ProductCategories.AnyAsync(x=>x.Name==updateProductCategoryCommand.Name && x.Id!=updateProductCategoryCommand.Id,cancellationToken))
            {
                return false;
            }

            return true;
        }
    }
}
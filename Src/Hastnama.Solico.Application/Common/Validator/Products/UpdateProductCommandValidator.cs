using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Products.Command.Update;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Update;

namespace Hastnama.Solico.Application.Common.Validator.Products
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public UpdateProductCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;

            CascadeMode = CascadeMode.Stop;


            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.ProductCategoryId)
                .NotEmpty()
                .NotNull()
                .MustAsync(ValidCategory).WithMessage(_localization.GetMessage(ResponseMessage.CategoryNotFound));
            
            
       
        }

        private async Task<bool> ValidCategory(long? productCategoryId, CancellationToken cancellationToken)
        {
            if (productCategoryId.HasValue)
            {
                if (await _context.ProductCategories.AnyAsync(x => x.Id == productCategoryId, cancellationToken)==false)
                {
                    return false;
                }
            }

            return true;
        }

     
    }
}
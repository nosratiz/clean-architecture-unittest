using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Update
{
    public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public UpdateProductCategoryCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await GetProductCategoryAsync(request, cancellationToken);

            if (category is null)
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.CategoryNotFound,
                        cancellationToken))));

            if (request.ParentId.HasValue && category.Children.Any())
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.CategoryHasChildren, cancellationToken))));
            }

            _mapper.Map(request, category);

            category.Slug = category.Slug.RemoveSpecialChar()
                .ToEnglishNumber()
                .GenerateSeoLink()
                .TrimEnd();

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<ProductCategory> GetProductCategoryAsync(UpdateProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            return await _context.ProductCategories
                .Include(x=>x.Children)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
    }
}
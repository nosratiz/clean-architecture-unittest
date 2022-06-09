using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Delete
{
    public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IProductCategoryServices _productCategoryServices;

        public DeleteProductCategoryCommandHandler(ISolicoDbContext context, ILocalization localization,
            IProductCategoryServices productCategoryServices)
        {
            _context = context;
            _localization = localization;
            _productCategoryServices = productCategoryServices;
        }

        public async Task<Result> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                if (await HasExistInProductAsync(id, cancellationToken))
                    return Result.Failed(new BadRequestObjectResult(
                        new ApiMessage(await _localization.GetMessage(ResponseMessage.CategoryInUse,
                            cancellationToken))));

                var category = await GetProductCategoryAsync(id, cancellationToken);

                if (category is null)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                        await _localization.GetMessage(ResponseMessage.CategoryNotFound, cancellationToken))));

                var success = await _productCategoryServices.DeleteWithChildren(category.Id, cancellationToken);

                if (success == false)
                {
                    return Result.Failed(new BadRequestObjectResult(
                        new ApiMessage(await _localization.GetMessage(ResponseMessage.ChildrenCategoryInUse,
                            cancellationToken))));
                }

                category.IsDeleted = true;
            }

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        private async Task<ProductCategory> GetProductCategoryAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ProductCategories.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        private async Task<bool> HasExistInProductAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.ProductCategoryId == id, cancellationToken);
        }
    }
}
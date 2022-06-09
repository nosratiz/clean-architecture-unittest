using System.Collections.Generic;
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

namespace Hastnama.Solico.Application.Shop.Products.Command.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(ISolicoDbContext context, ILocalization localization, IMapper mapper)
        {
            _context = context;
            _localization = localization;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await GetProductAsync(request, cancellationToken);

            if (product is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ProductNotFound,
                        cancellationToken))));
            }

            _context.ProductGalleries.RemoveRange(product.ProductGalleries);

            if (request.Galleries != null)
            {
                await AddImageToGalleryAsync(request, cancellationToken);
            }

            _context.ProductMedia.RemoveRange(product.ProductMedias);

            if (request.UpdateProductMedia != null)
            {
                var productMedias = _mapper.Map<List<ProductMedia>>(request.UpdateProductMedia);
                productMedias.ForEach(media =>
                {
                    media.ProductId = request.Id;
                } );

                await _context.ProductMedia.AddRangeAsync(productMedias, cancellationToken);
            }


            _mapper.Map(request, product);
            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        private async Task<Product> GetProductAsync(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductGalleries)
                .Include(x => x.ProductMedias)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #region Function

        private async Task AddImageToGalleryAsync(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            foreach (var gallery in request.Galleries)
            {
                await _context.ProductGalleries.AddAsync(new ProductGallery
                {
                    ProductId = request.Id,
                    Image = gallery
                }, cancellationToken);
            }
        }

        #endregion
    }
}
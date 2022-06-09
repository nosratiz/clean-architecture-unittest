using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetAdminProductQuery: IRequest<Result<ProductDto>>
    {
        public long Id { get; set; }
    }
    
    public class GetAdminProductQueryHandler : IRequestHandler<GetAdminProductQuery,Result<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IMapper _mapper;

        public GetAdminProductQueryHandler(ISolicoDbContext context, ILocalization localization, IMapper mapper)
        {
            _context = context;
            _localization = localization;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(GetAdminProductQuery request, CancellationToken cancellationToken)
        {
            var product = await GetProductAsync(request, cancellationToken);

            if (product is null)
            {
                return Result<ProductDto>.Failed(new NotFoundObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ProductNotFound,
                        cancellationToken))));
            }
            
            return Result<ProductDto>.SuccessFul(_mapper.Map<ProductDto>(product));
        }
        
        private async Task<Product> GetProductAsync(GetAdminProductQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductGalleries)
                .Include(x=>x.ProductMedias)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }
    }
}
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetProductPagedListQuery : PagingOptions, IRequest<PagedList<ProductDto>>
    {
        public long? ProductCategoryId { get; set; }
    }

    public class GetProductPagedListQueryHandler : PagingService<Product>,
        IRequestHandler<GetProductPagedListQuery, PagedList<ProductDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetProductPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductDto>> Handle(GetProductPagedListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Product> products = _context.Products
                .Include(x => x.ProductCategory);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                products = products.Where(x => x.Name.Contains(request.Search)
                                               || x.Description.Contains(request.Search) ||
                                               x.MaterialId.Contains(request.Search));
            }

            if (request.ProductCategoryId.HasValue)
                products = products.Where(x => x.ProductCategoryId == request.ProductCategoryId);


            if (!string.IsNullOrWhiteSpace(request.Sort))
            {
                switch (request.Sort.ToLower())
                {
                    case "materialid":
                        products = request.Desc
                            ? products.OrderByDescending(x => x.MaterialId)
                            : products.OrderBy(x => x.MaterialId);
                        break;

                    case "name":
                        products = request.Desc ? products.OrderByDescending(x => x.Name) : products.OrderBy(x => x.Name);
                        break;

                    case "materialtype":
                        products = request.Desc
                            ? products.OrderByDescending(x => x.MaterialType)
                            : products.OrderBy(x => x.MaterialType);
                        break;

                    default:
                        products = products.OrderBy(x => x.Id);
                        break;
                }

            }
          

            var productList = await GetPagedAsync(request.Page, request.Limit, products, cancellationToken);

            return productList.MapTo<ProductDto>(_mapper);
        }
    }
}
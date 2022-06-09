using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Queries
{
    public class GetProductCategoryPagedListQuery : PagingOptions, IRequest<PagedList<AdminProductCategoryDto>>
    {

    }

    public class GetProductCategoryPagedListQueryHandler :
        PagingService<ProductCategory>, IRequestHandler<GetProductCategoryPagedListQuery, PagedList<AdminProductCategoryDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetProductCategoryPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<AdminProductCategoryDto>> Handle(GetProductCategoryPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ProductCategory> category = _context.ProductCategories
                .Include(x=>x.Parent)
                .Where(x=>x.ParentId!=null);


            if (!string.IsNullOrWhiteSpace(request.Search))
                category = category.Where(x => x.Name.Contains(request.Search));


            var categoryList = await GetPagedAsync(request.Page, request.Limit, category,cancellationToken);


            return categoryList.MapTo<AdminProductCategoryDto>(_mapper);


        }
    }
}
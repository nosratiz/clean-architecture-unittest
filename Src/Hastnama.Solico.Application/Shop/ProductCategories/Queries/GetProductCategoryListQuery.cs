using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Queries
{
    public class GetProductCategoryListQuery : IRequest<List<ProductCategoryDto>>
    {
        public string Search { get; set; }
    }


    public class
        GetProductCategoryListQueryHandler : IRequestHandler<GetProductCategoryListQuery, List<ProductCategoryDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;


        public GetProductCategoryListQueryHandler(ISolicoDbContext context, ILanguageInfo languageInfo, IMapper mapper)
        {
            _context = context;
            _languageInfo = languageInfo;
            _mapper = mapper;
        }

        public async Task<List<ProductCategoryDto>> Handle(GetProductCategoryListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<ProductCategory> categoryList = _context.ProductCategories
                .Where(x => x.Lang == _languageInfo.LanguageCode && x.ParentId == null)
                .Include(x => x.Children)
                .ThenInclude(x => x.Children);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                categoryList = categoryList.Where(x =>
                    x.Name.Contains(request.Search) || x.Children.Any(c => c.Name.Contains(request.Search)));
            }

            return _mapper.Map<List<ProductCategoryDto>>(await categoryList.ToListAsync(cancellationToken));
        }
    }
}
using System;
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
using Microsoft.Extensions.Caching.Memory;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Queries
{
    public class GetSiteProductCategoryListQuery : IRequest<List<ProductCategoryDto>>
    {
    }

    public class GetSiteProductCategoryListQueryHandler : IRequestHandler<GetSiteProductCategoryListQuery,
            List<ProductCategoryDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;
        private readonly IMemoryCache _cache;

        public GetSiteProductCategoryListQueryHandler(ISolicoDbContext context, IMapper mapper,
            ILanguageInfo languageInfo, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
            _cache = cache;
        }

        public async Task<List<ProductCategoryDto>> Handle(GetSiteProductCategoryListQuery request,
            CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue($"{_languageInfo.LanguageCode}-ProductCategory",
                out List<ProductCategory> productCategories))
            {
                productCategories = await _context.ProductCategories
                    .Include(x=>x.Children)
                    .ThenInclude(x=>x.Children)
                    .Where(x => x.Lang == _languageInfo.LanguageCode && x.ParentId == null)
                    .ToListAsync(cancellationToken);

                _cache.Set($"{_languageInfo.LanguageCode}-ProductCategory", productCategories,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }

            return _mapper.Map<List<ProductCategoryDto>>(productCategories);
        }
    }
}
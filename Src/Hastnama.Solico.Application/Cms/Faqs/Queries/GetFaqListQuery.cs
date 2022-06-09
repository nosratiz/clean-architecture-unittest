using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hastnama.Solico.Application.Cms.Faqs.Queries
{
    public class GetFaqListQuery : IRequest<List<FaqDto>>
    {

    }

    public class GetFaqListQueryHandler : IRequestHandler<GetFaqListQuery, List<FaqDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILanguageInfo _languageInfo;

        public GetFaqListQueryHandler(ILanguageInfo languageInfo, IMemoryCache cache, IMapper mapper, ISolicoDbContext context)
        {
            _languageInfo = languageInfo;
            _cache = cache;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<FaqDto>> Handle(GetFaqListQuery request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue($"Faqs-{_languageInfo.LanguageCode}", out List<Faq> faqs))
            {
                faqs = await _context.Faqs.Where(x => x.Lang == _languageInfo.LanguageCode)
                    .ToListAsync(cancellationToken);


                _cache.Set($"Faqs-{_languageInfo.LanguageCode}", faqs, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
            }

            return _mapper.Map<List<FaqDto>>(faqs);
        }
    }
}
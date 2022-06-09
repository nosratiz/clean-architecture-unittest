using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Faqs.Queries
{
    public class GetFaqPagedListQuery : PagingOptions, IRequest<PagedList<FaqDto>>
    {

    }


    public class GetFaqPagedListQueryHandler : PagingService<Faq>, IRequestHandler<GetFaqPagedListQuery, PagedList<FaqDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;

        public GetFaqPagedListQueryHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<PagedList<FaqDto>> Handle(GetFaqPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Faq> faqs = _context.Faqs;

            if (!string.IsNullOrWhiteSpace(request.Search))
                faqs = faqs.Where(x => x.Question.Contains(request.Search) || x.Answer.Contains(request.Search));

            var faqPagedList = await GetPagedAsync(request.Page, request.Limit, faqs,cancellationToken);

            return faqPagedList.MapTo<FaqDto>(_mapper);
        }
    }
}
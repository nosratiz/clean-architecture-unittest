using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.SlideShows.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.SlideShows.Queries
{
    public class GetSliderPagedListQuery : PagingOptions, IRequest<PagedList<SlidShowDto>>
    {

    }

    public class GetSliderPagedListQueryHandler : PagingService<SlideShow>, IRequestHandler<GetSliderPagedListQuery, PagedList<SlidShowDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;

        public GetSliderPagedListQueryHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<PagedList<SlidShowDto>> Handle(GetSliderPagedListQuery request, CancellationToken cancellationToken)
        {
            var slider = _context.SlideShows.Where(x => x.Lang == _languageInfo.LanguageCode);

            if (!string.IsNullOrWhiteSpace(request.Search))
                slider = slider.Where(x => x.Name.Contains(request.Search));

            var sliderPagedList = await GetPagedAsync(request.Page, request.Limit, slider.OrderByDescending(x=>x.Id),cancellationToken);


            return sliderPagedList.MapTo<SlidShowDto>(_mapper);
        }
    }
}
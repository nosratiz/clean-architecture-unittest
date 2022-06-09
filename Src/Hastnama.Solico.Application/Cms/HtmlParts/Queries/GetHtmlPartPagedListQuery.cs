using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.HtmlParts.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.HtmlParts.Queries
{
    public class GetHtmlPartPagedListQuery : PagingOptions, IRequest<PagedList<HtmlPartDto>>
    {
    }

    public class GetHtmlPartPagedListQueryHandler : PagingService<HtmlPart>,
        IRequestHandler<GetHtmlPartPagedListQuery, PagedList<HtmlPartDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetHtmlPartPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<HtmlPartDto>> Handle(GetHtmlPartPagedListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<HtmlPart> htmlParts = _context.HtmlParts;


            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                htmlParts = htmlParts.Where(x => x.Title.Contains(request.Search));
            }

            var htmlPartList = await GetPagedAsync(request.Page, request.Limit, htmlParts, cancellationToken);

            return htmlPartList.MapTo<HtmlPartDto>(_mapper);
        }
    }
}
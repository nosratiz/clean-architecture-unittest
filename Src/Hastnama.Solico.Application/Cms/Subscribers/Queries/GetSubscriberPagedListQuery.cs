using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Subscribers.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Subscribers.Queries
{
    public class GetSubscriberPagedListQuery : PagingOptions, IRequest<PagedList<SubscriberDto>>
    {
    }

    public class GetSubscriberPagedListQueryHandler : PagingService<Subscribe>,
        IRequestHandler<GetSubscriberPagedListQuery, PagedList<SubscriberDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetSubscriberPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<SubscriberDto>> Handle(GetSubscriberPagedListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Subscribe> subscribers = _context.Subscribes;

            if (!string.IsNullOrWhiteSpace(request.Search))
                subscribers = subscribers.Where(x => x.Email == request.Search);

            var subscriberList = await GetPagedAsync(request.Page, request.Limit, subscribers, cancellationToken);

            return subscriberList.MapTo<SubscriberDto>(_mapper);
        }
    }
}
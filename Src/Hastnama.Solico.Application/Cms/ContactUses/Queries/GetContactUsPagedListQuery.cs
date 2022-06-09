using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.ContactUses.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.ContactUses.Queries
{
    public class GetContactUsPagedListQuery : PagingOptions, IRequest<PagedList<ContactUsDto>>
    {

    }

    public class GetContactUsPagedListQueryHandler : PagingService<ContactUs>, IRequestHandler<GetContactUsPagedListQuery, PagedList<ContactUsDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetContactUsPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<ContactUsDto>> Handle(GetContactUsPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ContactUs> contactUses = _context.ContactUs;

            if (!string.IsNullOrWhiteSpace(request.Search))
                contactUses = contactUses.Where(x =>
                    x.Email.Contains(request.Search) || x.Name.Contains(request.Search) ||
                    x.Message.Contains(request.Search));

            var contactUsList = await GetPagedAsync(request.Page, request.Limit, contactUses,cancellationToken);

            return contactUsList.MapTo<ContactUsDto>(_mapper);
        }
    }
}
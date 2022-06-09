using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.CustomerConsults.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.CustomerConsults.Queries
{
    public class GetConsultPagedListQuery : PagingOptions, IRequest<PagedList<CustomerConsultDto>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class GetConsultPagedListQueryHandler : PagingService<CustomerConsult>,
        IRequestHandler<GetConsultPagedListQuery, PagedList<CustomerConsultDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetConsultPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PagedList<CustomerConsultDto>> Handle(GetConsultPagedListQuery request,
            CancellationToken cancellationToken)
        {
            request.FromDate ??= new DateTime();
            request.ToDate ??= DateTime.Now;
          
            IQueryable<CustomerConsult> customerConsults =
                _context.CustomerConsults
                    .Include(x=>x.Customer)
                    .OrderByDescending(x=>x.CreateDate)
                    .Where(x => x.CreateDate >= request.FromDate && x.CreateDate <= request.ToDate);


            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                customerConsults = customerConsults.Where(x =>
                    x.Customer.FullName.Contains(request.Search) || x.Customer.Mobile.Contains(request.Search));
            }

            var customerConsultList =
                await GetPagedAsync(request.Page, request.Limit, customerConsults, cancellationToken);

            return customerConsultList.MapTo<CustomerConsultDto>(_mapper);
        }
    }
}
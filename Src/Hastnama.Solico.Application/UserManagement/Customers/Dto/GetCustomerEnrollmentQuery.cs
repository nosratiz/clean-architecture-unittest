using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Dto
{
    public class GetCustomerEnrollmentPagedListQuery : PagingOptions, IRequest<PagedList<CustomerEnrollmentDto>>
    {
    }

    public class GetCustomerEnrollmentPagedListQueryHandler : PagingService<CustomerEnrollment>,
        IRequestHandler<GetCustomerEnrollmentPagedListQuery, PagedList<CustomerEnrollmentDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomerEnrollmentPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<CustomerEnrollmentDto>> Handle(GetCustomerEnrollmentPagedListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<CustomerEnrollment> customerEnrollments = _context.CustomerEnrollments.Where(x=>x.IsDone==false);

            var customerEnrolmentList =
                await GetPagedAsync(request.Page, request.Limit, customerEnrollments, cancellationToken);

            return customerEnrolmentList.MapTo<CustomerEnrollmentDto>(_mapper);
        }
    }
}
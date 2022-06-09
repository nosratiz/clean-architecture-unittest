using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.UserManagement.Customers.Queries
{
    public class GetCustomerPagedListQuery : PagingOptions, IRequest<PagedList<CustomerDto>>
    {
    }

    public class GetCustomerPagedListQueryHandler : PagingService<Customer>,
        IRequestHandler<GetCustomerPagedListQuery, PagedList<
            CustomerDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomerPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<CustomerDto>> Handle(GetCustomerPagedListQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Customer> customers = _context.Customers;

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                customers = customers.Where(x =>
                    x.Address.Contains(request.Search) || x.FullName.Contains(request.Search) ||
                    x.Phone.Contains(request.Search) || x.Mobile.Contains(request.Search) || x.SolicoCustomerId.Contains(request.Search));
            }

            if (!string.IsNullOrWhiteSpace(request.Sort))
            {
                switch (request.Sort.ToLower())
                {
                    case "solicocustomerid":
                        customers = request.Desc
                            ? customers.OrderByDescending(x => x.SolicoCustomerId)
                            : customers.OrderBy(x => x.SolicoCustomerId);
                        break;
                    case "fullname":
                        customers = request.Desc
                            ? customers.OrderByDescending(x => x.FullName)
                            : customers.OrderBy(x => x.FullName);

                        break;

                    case "citycode":
                        customers = request.Desc
                            ? customers.OrderByDescending(x => x.CityCode)
                            : customers.OrderBy(x => x.CityCode);
                        break;

                    default:
                        customers = customers.OrderByDescending(x => x.SyncDate);
                        break;
                }

            }
           
            var customerPagedList = await GetPagedAsync(request.Page, request.Limit, customers, cancellationToken);


            return customerPagedList.MapTo<CustomerDto>(_mapper);
        }
    }
}
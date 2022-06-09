using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Queries
{
    public class GetCustomerOpenItemsQuery : PagingOptions, IRequest<PagedList<CustomerOpenItemDto>>
    {
    }

    public class GetCustomerOpenItemsQueryHandler : PagingService<CustomerOpenItem>,
        IRequestHandler<GetCustomerOpenItemsQuery, PagedList<CustomerOpenItemDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetCustomerOpenItemsQueryHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<PagedList<CustomerOpenItemDto>> Handle(GetCustomerOpenItemsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<CustomerOpenItem> customerOpenItems =
                _context.CustomerOpenItems.Where(x => x.CustomerId == Guid.Parse(_currentCustomerService.Id));

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                customerOpenItems = customerOpenItems.Where(x => x.DocumentNumber.Contains(request.Search));
            }

            var customerOpenItemsList =
                await GetPagedAsync(request.Page, request.Limit, customerOpenItems, cancellationToken);

            return customerOpenItemsList.MapTo<CustomerOpenItemDto>(_mapper);
        }
    }
}
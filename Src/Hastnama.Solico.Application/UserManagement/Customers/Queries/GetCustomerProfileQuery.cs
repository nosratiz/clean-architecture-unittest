using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.UserManagement.Customers.Queries
{
    public class GetCustomerProfileQuery : IRequest<CustomerDto>
    {
    }

    public class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, CustomerDto>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IMapper _mapper;

        public GetCustomerProfileQueryHandler(ISolicoDbContext context, ICurrentCustomerService currentCustomerService,
            IMapper mapper)
        {
            _context = context;
            _currentCustomerService = currentCustomerService;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var customer =
                await _context.Customers
                    .SingleOrDefaultAsync(x => x.Id == Guid.Parse(_currentCustomerService.Id),
                    cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
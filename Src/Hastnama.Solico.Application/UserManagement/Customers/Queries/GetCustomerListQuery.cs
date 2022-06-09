using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Customers.Queries
{
    public class GetCustomerListQuery : IRequest<List<Customer>>
    {
    }

    public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, List<Customer>>
    {
        private readonly ISolicoDbContext _context;

        public GetCustomerListQueryHandler(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }
    }
}
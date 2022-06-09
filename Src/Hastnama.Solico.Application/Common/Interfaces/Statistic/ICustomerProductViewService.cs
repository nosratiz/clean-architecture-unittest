using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Domain.Models.Statistic;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Interfaces.Statistic
{
    public interface ICustomerProductViewService
    {
        Task UpdateViewCount(Guid CustomerId, long ProductId,CancellationToken cancellationToken);
    }


    public class CustomerProductViewService : ICustomerProductViewService
    {
        private readonly ISolicoDbContext _context;

        public CustomerProductViewService(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task UpdateViewCount(Guid CustomerId, long ProductId,CancellationToken cancellationToken)
        {
            var CustomerView = await _context.CustomerProductViews
                .FirstOrDefaultAsync(x => x.ProductId == ProductId 
                                          && x.CustomerId == CustomerId,cancellationToken);

            if (CustomerView is null)
            {
                await _context.CustomerProductViews.AddAsync(new CustomerProductView
                {
                    CustomerId = CustomerId,
                    ProductId = ProductId,
                    ViewCount = 1
                });
            }
            else
            {
                CustomerView.ViewCount += 1;
            }

            await _context.SaveAsync(cancellationToken);
        }
    }
}
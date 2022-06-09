using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Shop.Products.Queries
{
    public class GetProductListQuery : IRequest<List<Product>>
    {
        public long? ProductCategoryId { get; set; }

        public string Search { get; set; }
    }


    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, List<Product>>
    {
        private readonly ISolicoDbContext _context;

        public GetProductListQueryHandler(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> products = _context.Products
                .Include(x => x.ProductCategory);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                products = products.Where(x => x.Name.Contains(request.Search)
                                               || x.Description.Contains(request.Search) ||
                                               x.MaterialId.Contains(request.Search));
            }

            if (request.ProductCategoryId.HasValue)
                products = products.Where(x => x.ProductCategoryId == request.ProductCategoryId);


            return await products.ToListAsync(cancellationToken);
        }
    }
}
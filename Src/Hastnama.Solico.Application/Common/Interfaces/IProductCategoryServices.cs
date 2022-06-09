using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface IProductCategoryServices
    {
        Task<bool> DeleteWithChildren(long id, CancellationToken cancellationToken);
    }

    public class ProductCategoryServices : IProductCategoryServices
    {
        private readonly ISolicoDbContext _context;

        public ProductCategoryServices(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteWithChildren(long id, CancellationToken cancellationToken)
        {
            var category = await _context.ProductCategories
                .Include(x => x.Children)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category.Children.Any())
            {
                foreach (var Children in category.Children)
                {
                    if (await _context.Products.AnyAsync(x => x.Id == Children.Id, cancellationToken))
                    {
                        return false;
                    }

                    Children.IsDeleted = true;
                    await DeleteWithChildren(Children.Id, cancellationToken);
                }
            }

            return true;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.Logs;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.Statistic;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface ISolicoDbContext
    {
        DbSet<AppSetting> AppSettings { get; set; }
        
        DbSet<Faq> Faqs { get; set; }
        
        DbSet<ContactUs> ContactUs { get; set; }
        
        DbSet<HtmlPart> HtmlParts { get; set; }
        
        DbSet<Subscribe> Subscribes { get; set; }
        
        DbSet<SlideShow> SlideShows { get; set; }
        
        DbSet<Role> Roles { get; set; }
        
        DbSet<UserFile> UserFiles { get; set; }
        
        DbSet<UserToken> UserTokens { get; set; }
        
        DbSet<User> Users { get; set; }
        
        DbSet<UserAddress> UserAddresses { get; set; }

        DbSet<DailyStatistic> DailyStatistics { get; set; }
        
        
        DbSet<Company> Companies { get; set; }
        
        DbSet<Product> Products { get; set; }
        
        DbSet<ProductCategory> ProductCategories { get; set; }
        
        DbSet<ProductGallery> ProductGalleries { get; set; }
        
        DbSet<Order> Orders { get; set; }
        
        DbSet<OrderItem> OrderItems { get; set; }
        
        DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        
        DbSet<BankTransaction> BankTransactions { get; set; }
        
        DbSet<SolicoEventLog> SolicoEventLogs { get; set; }
        
        DbSet<Customer> Customers { get; set; }
        
        DbSet<CustomerProductView> CustomerProductViews { get; set; }
        
        DbSet<CustomerEnrollment> CustomerEnrollments { get; set; }
        
        DbSet<CustomerProductPrice> CustomerProductPrices { get; set; }
        
        DbSet<ProductMedia> ProductMedia { get; set; }
        
        DbSet<Message> messages { get; set; }
        
        DbSet<UserMessage> UserMessages { get; set; }
        
        DbSet<CustomerConsult> CustomerConsults { get; set; }
        
        DbSet<CustomerOpenItem> CustomerOpenItems { get; set; }

        Task SaveAsync(CancellationToken cancellationToken);
    }
}
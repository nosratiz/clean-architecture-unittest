using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.Logs;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.Statistic;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Persistence.Context
{
    public class SolicoContext : DbContext, ISolicoDbContext
    {
        public SolicoContext()
        {
        }

        public SolicoContext(DbContextOptions<SolicoContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=185.165.40.218;Initial Catalog =KaleDb;MultipleActiveResultSets=true;User ID=sa;Password=123qwe!@#QWE;Max Pool Size=1000;Pooling=true");
            }
        }


        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<HtmlPart> HtmlParts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<SlideShow> SlideShows { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<DailyStatistic> DailyStatistics { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<SolicoEventLog> SolicoEventLogs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerProductView> CustomerProductViews { get; set; }

        public DbSet<CustomerEnrollment> CustomerEnrollments { get; set; }

        public DbSet<CustomerProductPrice> CustomerProductPrices { get; set; }

        public DbSet<ProductMedia> ProductMedia { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        
        public DbSet<CustomerConsult> CustomerConsults { get; set; }
        public DbSet<CustomerOpenItem> CustomerOpenItems { get; set; }
        public Task SaveAsync(CancellationToken cancellationToken) => base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolicoContext).Assembly);
    }
}
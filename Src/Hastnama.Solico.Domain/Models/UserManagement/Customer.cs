using System;
using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.Statistic;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class Customer
    {
        public Customer()
        {
            UserTokens = new HashSet<UserToken>();
            OrderItems = new HashSet<OrderItem>();
            CustomerProductViews = new HashSet<CustomerProductView>();
            CustomerProductPrices = new HashSet<CustomerProductPrice>();
            UserMessages = new HashSet<UserMessage>();
            CustomerConsults = new HashSet<CustomerConsult>();
            CustomerOpenItems = new HashSet<CustomerOpenItem>();
        }
        
        public Guid Id { get; set; }
        public string SolicoCustomerId { get; set; }

        public string PayerId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Password { get; set; }

        public string ActiveCode { get; set; }
        
        public string Country { get; set; }

        public string Region { get; set; }

        public string CityCode { get; set; }

        public string Address { get; set; }

        public string AddressNumber { get; set; }

        public double? CreditUsed { get; set; }

        public double? CreditLimit { get; set; }

        public double? CreditExposure { get; set; }

        public bool IsShowInLastSync { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        public DateTime SyncDate { get; set; }

        public DateTime? ExpiredVerification { get; set; }
        
        
        public virtual ICollection<UserToken> UserTokens { get; }
        
        public virtual ICollection<OrderItem> OrderItems { get; }
        
        public virtual ICollection<CustomerProductView> CustomerProductViews { get;  }
        
        public virtual ICollection<CustomerProductPrice> CustomerProductPrices { get; }
        
        public virtual ICollection<UserMessage> UserMessages { get; }
        
        public virtual ICollection<CustomerConsult> CustomerConsults { get; }
        
        public virtual ICollection<CustomerOpenItem> CustomerOpenItems { get; }
    }
}
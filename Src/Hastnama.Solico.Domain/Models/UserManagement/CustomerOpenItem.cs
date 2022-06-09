using System;
using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.Shop;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class CustomerOpenItem
    {
        public CustomerOpenItem()
        {
            BankTransactions = new HashSet<BankTransaction>();
        }
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string DocumentNumber { get; set; }

        public double Amount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; }
        
        public virtual Customer Customer { get; set; }
        
        public virtual ICollection<BankTransaction> BankTransactions { get; }
        
    }
}
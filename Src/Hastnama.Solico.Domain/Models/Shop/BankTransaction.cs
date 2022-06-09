using System;
using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class BankTransaction
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        
        public Guid? CustomerOpenItemId { get; set; }
        public long Price { get; set; }
        public long? RefId { get; set; }
        public int Status { get; set; }

        public string Token { get; set; }

        public DateTime CreateDate { get; set; }
        public virtual Order Order { get; set; }
        
        public virtual CustomerOpenItem CustomerOpenItem { get; set; }
        
    }
}
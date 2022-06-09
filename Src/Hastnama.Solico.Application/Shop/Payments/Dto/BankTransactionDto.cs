using System;

namespace Hastnama.Solico.Application.Shop.Payments.Dto
{
    public class BankTransactionDto
    {
        public string DocumentNumber { get; set; }

        public string OrderNumber { get; set; }
        
        public long Price { get; set; }
        
        public long? RefId { get; set; }
        
        public int Status { get; set; }

        public string Token { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
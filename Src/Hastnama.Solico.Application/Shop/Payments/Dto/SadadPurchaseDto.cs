using System;

namespace Hastnama.Solico.Application.Shop.Payments.Dto
{
    public class SadadPurchaseDto
    {
        public string MerchantId { get; set; }

        public string TerminalId { get; set; }

        public long OrderId { get; set; }

        public long Amount { get; set; }

        public DateTime LocalDateTime { get; set; }

        public string ReturnUrl { get; set; }

        public string SignData { get; set; }
    }
}
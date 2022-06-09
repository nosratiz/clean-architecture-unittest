namespace Hastnama.Solico.Application.UserManagement.Customers.Dto
{
    public class CustomerCreditDto
    {
        public double Limit { get; set; }

        public double Exposure { get; set; }

        public double CreditLimitUsed { get; set; }

        public string Currency { get; set; }
    }
}
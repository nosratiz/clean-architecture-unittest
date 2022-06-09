namespace Hastnama.Solico.Common.Helper.Claims.Customer
{
    public interface ICurrentCustomerService
    {
        public string FullName { get; set; }

        public string Id { get; set; }

        public string CustomerId { get; set; }

        public string Mobile { get; set; }
    }
}
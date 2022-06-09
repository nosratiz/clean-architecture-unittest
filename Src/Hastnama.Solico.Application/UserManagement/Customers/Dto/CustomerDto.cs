using System;

namespace Hastnama.Solico.Application.UserManagement.Customers.Dto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }

        public string SolicoCustomerId { get; set; }
        
        public string PayerId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string CityCode { get; set; }

        public string Address { get; set; }

        public string AddressNumber { get; set; }
        
        public double? CreditUsed { get; set; }

        public double? CreditLimit { get; set; }

        public double? CreditExposure { get; set; }

        public bool IsActive { get; set; }
        
        public DateTime SyncDate { get; set; }
    }
}
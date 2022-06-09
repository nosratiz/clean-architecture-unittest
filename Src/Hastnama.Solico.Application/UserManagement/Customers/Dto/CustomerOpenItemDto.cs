using System;

namespace Hastnama.Solico.Application.UserManagement.Customers.Dto
{
    public class CustomerOpenItemDto
    {
        public Guid Id { get; set; }

        public string DocumentNumber { get; set; }

        public double Amount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; }
    }
}
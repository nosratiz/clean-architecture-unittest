using System;

namespace Hastnama.Solico.Application.UserManagement.Customers.Dto
{
    public class CustomerEnrollmentDto
    {
        public long Id { get; set; }

        public string SolicoCustomerId { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDone { get; set; }
    }
}
using System;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Domain.Models.Cms
{
    public class CustomerConsult
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsSettle { get; set; }

        public bool IsDelete { get; set; }

        public Customer Customer { get; set; }
    }
}
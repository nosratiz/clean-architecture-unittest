using System;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class CustomerEnrollment
    {
        public long Id { get; set; }

        public string SolicoCustomerId { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDone { get; set; }
    }
}
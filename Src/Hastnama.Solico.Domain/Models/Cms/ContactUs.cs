using System;

namespace Hastnama.Solico.Domain.Models.Cms
{
    public class ContactUs
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
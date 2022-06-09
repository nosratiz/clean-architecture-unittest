using System;

namespace Hastnama.Solico.Domain.Models.Cms
{
    public class Subscribe
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
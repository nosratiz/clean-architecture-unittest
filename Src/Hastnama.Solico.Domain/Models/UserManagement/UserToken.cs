using System;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class UserToken
    {
        public Guid Id { get; set; }

        public long? UserId { get; set; }
        public Guid? CustomerId { get; set; }

        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string Browser { get; set; }
        public string Token { get; set; }


        public bool IsUsed { get; set; }

        public DateTime ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }


        public virtual User User { get; set; }
        
        public virtual Customer Customer { get; set; }
    }
}
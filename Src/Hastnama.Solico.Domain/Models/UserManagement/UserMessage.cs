using System;
using Hastnama.Solico.Domain.Models.Cms;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class UserMessage
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public long? UserId { get; set; }

        public Guid MessageId { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime? SeenDate { get; set; }

        public bool IsAdmin { get; set; }
        public bool CustomerHasDeleted { get; set; }

        public bool UserHasDeleted { get; set; }

        public virtual Message Message { get; set; }
        
        public virtual Customer Customer { get; set; }
        
        public virtual User User { get; set; }
    }
}
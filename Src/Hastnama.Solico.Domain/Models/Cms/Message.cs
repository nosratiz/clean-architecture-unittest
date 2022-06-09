using System;
using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Domain.Models.Cms
{
    public class Message
    {
        public Message()
        {
            UserMessages = new HashSet<UserMessage>();
            Children = new HashSet<Message>();
        }

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string File { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Message Parent { get; set; }

        public virtual ICollection<Message> Children { get; }
        public virtual ICollection<UserMessage> UserMessages { get; }
    }
}
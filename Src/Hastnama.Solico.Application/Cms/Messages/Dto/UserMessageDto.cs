using System;

namespace Hastnama.Solico.Application.Cms.Messages.Dto
{
    public class UserMessageDto
    {
        public Guid Id { get; set; }
        
        public Guid MessageId { get; set; }
        
        public DateTime SendDate { get; set; }

        public DateTime? SeenDate { get; set; }

        public string UserName { get; set; }

        public string CustomerName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string File { get; set; }
        
        
        public bool IsAdmin { get; set; }

    }
}
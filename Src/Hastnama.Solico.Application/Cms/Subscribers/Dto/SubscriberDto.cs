using System;

namespace Hastnama.Solico.Application.Cms.Subscribers.Dto
{
    public class SubscriberDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
       
    }
}
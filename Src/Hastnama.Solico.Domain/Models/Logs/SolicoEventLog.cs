using System;

namespace Hastnama.Solico.Domain.Models.Logs
{
    public class SolicoEventLog
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public bool IsSuccess { get; set; } 
        
        public DateTime CreateDate { get; set; }
    }
}
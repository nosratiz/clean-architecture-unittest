using System;

namespace Hastnama.Solico.Application.Cms.CustomerConsults.Dto
{
    public class CustomerConsultDto
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }
        

        public DateTime CreateDate { get; set; }

        public bool IsSettle { get; set; }

    }
}
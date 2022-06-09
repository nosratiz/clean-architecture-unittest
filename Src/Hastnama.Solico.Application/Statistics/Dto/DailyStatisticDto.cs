using System;

namespace Hastnama.Solico.Application.Statistics.Dto
{
    public class DailyStatisticDto
    {
        public Guid Id { get; set; }

        public int RegisterUser { get; set; }

        public int TotalOrder { get; set; }

        public long TotalRevenue { get; set; }

        public DateTime Date { get; set; }
    }
}
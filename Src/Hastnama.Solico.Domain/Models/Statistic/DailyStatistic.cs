using System;

namespace Hastnama.Solico.Domain.Models.Statistic
{
    public class DailyStatistic
    {
        public Guid Id { get; set; }

        public int RegisterUser { get; set; }

        public int TotalView { get; set; }

        public int TotalOrder { get; set; }

        public long TotalRevenue { get; set; }

        public DateTime Date { get; set; }
    }
}
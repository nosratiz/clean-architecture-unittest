namespace Hastnama.Solico.Domain.Models.Cms
{
    public class Faq
    {
        public long Id { get; set; }

        public string Answer { get; set; }
        public string Question { get; set; }
        public string Lang { get; set; }
    }
}
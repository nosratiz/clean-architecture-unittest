namespace Hastnama.Solico.Common.Helper
{
    public interface IRequestMeta
    {
        string Ip { get; set; }
        string UserAgent { get; set; }
        string Browser { get; set; }
        string Os { get; set; }
        string Device { get; set; }
    }

    public class RequestMeta : IRequestMeta
    {
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string Browser { get; set; }
        public string Os { get; set; }
        public string Device { get; set; }
    }
}
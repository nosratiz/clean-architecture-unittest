using System.ComponentModel.DataAnnotations;

namespace Hastnama.Solico.Common.Helper.Pagination
{
    public class PagingOptions
    {
        [Range(1, 9999)] public int Page { get; set; } = 1;

        [Range(1, 100)] public int Limit { get; set; } = 10;

        public string Search { get; set; }

        public string Sort { get; set; }

        public bool Desc { get; set; }
    }
}
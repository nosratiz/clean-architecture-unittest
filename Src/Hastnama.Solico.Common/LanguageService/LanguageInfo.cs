using System.Globalization;

namespace Hastnama.Solico.Common.LanguageService
{
    public class LanguageInfo : ILanguageInfo
    {
        public string LanguageCode { get; set; } = "fa";

        public CultureInfo CultureInfo { get; set; }
    }
}
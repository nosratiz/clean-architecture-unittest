using System.Globalization;

namespace Hastnama.Solico.Common.LanguageService
{
    public interface ILanguageInfo
    {
        string LanguageCode { get; set; }

        CultureInfo CultureInfo { get; set; }
    }
}
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Common.LanguageService;

namespace Hastnama.Solico.Common.Localization
{
    public interface ILocalization
    {
        Task<string> GetMessage(string key, CancellationToken cancellationToken);

        string GetMessage(string Key);
    }

    public class Localization : ILocalization
    {
        private readonly ILanguageInfo _languageInfo;

        public Localization(ILanguageInfo languageInfo)
        {
            _languageInfo = languageInfo;
        }

        public async Task<string> GetMessage(string key, CancellationToken cancellationToken)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory() + $"/Resources/ResponseMessage-{_languageInfo.LanguageCode}.json");

            var jsonData = await File.ReadAllTextAsync(filepath, cancellationToken);

            var localization = JsonSerializer.Deserialize<Resources>(jsonData);

            if (localization != null)
            {
                var message = localization.Data.FirstOrDefault(x => x.Key == key);

                return message?.Template;
            }

            return "خطای ناشناخته سمت سرور";
        }

        public string GetMessage(string key)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory() + $"/Resources/ResponseMessage-{_languageInfo.LanguageCode}.json");

            var jsonData = File.ReadAllText(filepath);

            var localization = JsonSerializer.Deserialize<Resources>(jsonData);

            if (localization != null)
            {
                var message = localization.Data.FirstOrDefault(x => x.Key == key);

                return message?.Template;
            }

            return "خطای ناشناخته سمت سرور";

        }
    }
}
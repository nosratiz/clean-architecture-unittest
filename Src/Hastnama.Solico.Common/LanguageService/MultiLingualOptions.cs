using System.Collections.Generic;

namespace Hastnama.Solico.Common.LanguageService
{
    public class MultiLingualOptions
    {
        public string DefaultLanguage { get; set; }

        public List<string> AcceptedLanguages { get; set; }
    }
}
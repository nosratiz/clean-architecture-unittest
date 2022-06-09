using System.Collections.Generic;

namespace Hastnama.Solico.Common.Localization
{
    public class Resources
    {
        public List<ErrorMessage> Data { get; set; }
    }

    public class ErrorMessage
    {
        public string Key { get; set; }
        public string Template { get; set; }
    }
}
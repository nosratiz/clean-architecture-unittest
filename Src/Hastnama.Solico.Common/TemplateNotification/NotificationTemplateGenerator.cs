using System.IO;
using System.Text;
using DotLiquid;
using Hastnama.Solico.Common.Environment;
using Microsoft.AspNetCore.Hosting;

namespace Hastnama.Solico.Common.TemplateNotification
{
    public class NotificationTemplateGenerator : INotificationTemplateGenerator
    {
        private readonly IWebHostEnvironment _environment;

        public NotificationTemplateGenerator(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string CreateConfirmCode(ConfirmCodeTemplate confirmCode)
        {
            Template template =
                Template.Parse(File.ReadAllText(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Documents, "ConfirmCode.txt"),
                    Encoding.UTF8));

            return template.Render(Hash.FromAnonymousObject(confirmCode));
        }

      
    }
}
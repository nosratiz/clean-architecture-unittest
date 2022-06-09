using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Hastnama.Solico.Common.Environment
{
    public class ApplicationBootstrapper : IApplicationBootstrapper
    {
        private readonly IWebHostEnvironment _environment;

        public ApplicationBootstrapper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private void EnsureFoldersCreated()
        {
            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Avatars)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Avatars));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Images)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Images));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Videos)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Videos));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Musics)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Musics));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Documents)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Documents));

            if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Others)))
                Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, ApplicationStaticPath.Others));
        }

        public void Initial()
        {
            EnsureFoldersCreated();
        }
    }
}
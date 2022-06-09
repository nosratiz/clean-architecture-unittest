using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.LanguageService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hastnama.Solico.Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MultiLingualMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILanguageInfo _languageInfo;
        private readonly MultiLingualOptions _options;

        public MultiLingualMiddleware(RequestDelegate next,
            IOptions<MultiLingualOptions> options,

            ILanguageInfo languageInfo)
        {
            _next = next;
            _languageInfo = languageInfo;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var defaultLanguage = _options.DefaultLanguage;
            try
            {
                if (!httpContext.HasLanguage())
                {
                    httpContext.Response.SetLanguage(defaultLanguage);
                    var culture = new CultureInfo(defaultLanguage);
                    _languageInfo.LanguageCode = defaultLanguage;
                    _languageInfo.CultureInfo = culture;
                }
                else
                {
                
                    var requestLanguage = httpContext.Request.GetLanguage();

                    if (_options.AcceptedLanguages.Any(lang => lang == requestLanguage))
                    {
                        httpContext.Response.SetLanguage(requestLanguage);
                        var culture = new CultureInfo(requestLanguage);
                        _languageInfo.LanguageCode = requestLanguage;
                        _languageInfo.CultureInfo = culture;
                    }
                    else
                    {
                        httpContext.Response.SetLanguage(defaultLanguage);
                        var culture = new CultureInfo(defaultLanguage);
                        _languageInfo.LanguageCode = defaultLanguage;
                        _languageInfo.CultureInfo = culture;
                    }
                }
            }
            catch (Exception)
            {
                //
            }


            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MultiLingualMiddlewareExtensions
    {
        public static IApplicationBuilder UseMultiLingual(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MultiLingualMiddleware>();
        }

        public static IApplicationBuilder UseMultiLingual(this IApplicationBuilder app, MultiLingualOptions options)
        {
            return app.UseMiddleware<MultiLingualMiddleware>(Options.Create(options));
        }
    }
}
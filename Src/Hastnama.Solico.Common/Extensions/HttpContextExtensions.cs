using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Hastnama.Solico.Common.Extensions
{
    public static class HttpContextExtensions
    {
        
        public static async Task WriteError(this HttpContext httpContext, object error)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.Headers.Add("Content-Type", "application/json");

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }

        public static async Task WriteError(this HttpContext httpContext, object error, int statusCode)
        {
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.Headers.Add("Content-Type", "application/json");

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }

        public static async Task WriteJsonAsync(this HttpContext httpContext, object obj)
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.Headers.Add("Content-Type", "application/json");

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(obj));
        }


        public static bool IsGet(this HttpRequest request) => request.Method == "Get";

        public static bool IsPost(this HttpRequest request) => request.Method == "POST";

        public static bool IsPut(this HttpRequest request) => request.Method == "PUT";

        public static bool IsPatch(this HttpRequest request) => request.Method == "PATCH";

        public static bool IsDelete(this HttpRequest request) => request.Method == "DELETE";

        public static bool IsOptions(this HttpRequest request) => request.Method == "OPTIONS";

        public static bool IsHead(this HttpRequest request) => request.Method == "HEAD";

        public static bool HasFileCount(this HttpContext context) => !string.IsNullOrEmpty(context.Request.Headers["X-MultiSelect"]);

        public static string GetFilesCount(this HttpContext context) => !string.IsNullOrEmpty(context.Request.Headers["X-MultiSelect"])
            ? (string)context.Request.Headers["X-MultiSelect"] : string.Empty;

        public static bool HasAuthorization(this HttpContext context) => !string.IsNullOrEmpty(context.Request.Headers["Authorization"]);

        public static string GetAuthorizationToken(this HttpContext context)
        {
            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"])) return string.Empty;

            const string authenticationSchema = "Bearer";

            var userToken = context.Request.Headers["Authorization"].ToString();

            return userToken.Replace($"{authenticationSchema} ", "");
        }

        public static bool HasLanguage(this HttpContext httpContext) => !string.IsNullOrWhiteSpace(httpContext.Request.Headers["Accept-Language"]);

        public static string GetLanguage(this HttpRequest request) => request.Headers["Accept-Language"];

        public static void SetLanguage(this HttpResponse response, string language)
        {
            response.Headers["Accept-Language"] = language;
        }

     
    }
}
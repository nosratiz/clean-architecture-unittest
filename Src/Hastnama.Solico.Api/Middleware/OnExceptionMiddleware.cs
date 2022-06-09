using Hastnama.Solico.Common.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hastnama.Solico.Api.Middleware
{
    /// <summary>
    ///  OnExceptionMiddleware , Produce Custom message during Internal Server Exception
    /// </summary>
    public class OnExceptionMiddleware : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public OnExceptionMiddleware(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            var error = new ApiMessage();

            if (_env.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.StackTrace;
            }
            else
            {
                error.Message = "خطای سمت سرور";
                error.Detail = context.Exception.Message;
            }
            Log.Error(context.Exception, context.Exception.Message, context.Exception.StackTrace);

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hastnama.Solico.Api.Middleware
{
    public class MembershipMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSetting _options;
        private readonly ILocalization _localization;

        public MembershipMiddleware(RequestDelegate next,
            IOptions<JwtSetting> options, ILocalization localization)
        {
            _next = next;
            _localization = localization;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.HasAuthorization())
            {
                var token = httpContext.GetAuthorizationToken();
                var secretKey = Encoding.UTF8.GetBytes(_options.SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true, //default : false
                    ValidAudience = _options.ValidAudience,
                    ValidateIssuer = true, //default : false
                    ValidIssuer = _options.ValidIssuer,

                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                };

                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);
                    httpContext.User = claimsPrincipal;
                    
                    
                    var context =
                        (ISolicoDbContext)httpContext.RequestServices.GetService(typeof(ISolicoDbContext));


                    if (context != null)
                    {
                        var  userToken = await context.UserTokens.FirstOrDefaultAsync(x => x.Token == token);
                  
                        if (userToken is null)
                        {
                            await httpContext.WriteError(new ApiMessage( _localization.GetMessage(ResponseMessage.InvalidToken)),
                                StatusCodes.Status401Unauthorized);
                            return;
                        }

                        if (userToken.IsUsed)
                        {
                            await httpContext.WriteError(new ApiMessage(_localization.GetMessage(ResponseMessage.TokenExpired)),
                                StatusCodes.Status401Unauthorized);
                            return;
                        }
                    }
                }
              
                catch (Exception ex)
                {
                    await httpContext.WriteError(ex.Message, StatusCodes.Status401Unauthorized);
                }

           
            }

            await _next(httpContext);
        }
    }
}
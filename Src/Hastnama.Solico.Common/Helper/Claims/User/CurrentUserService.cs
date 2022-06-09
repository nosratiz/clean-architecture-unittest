using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Hastnama.Solico.Common.Helper.Claims.User
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
            IsAuthenticated = UserId != null;
            FullName = httpContextAccessor.HttpContext?.User?.FindFirstValue("fullName");
            Email = httpContextAccessor.HttpContext?.User?.FindFirstValue("Email");
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue("UserName");
        }

        public string UserId { get; }
        public string FullName { get; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; }
    }
}
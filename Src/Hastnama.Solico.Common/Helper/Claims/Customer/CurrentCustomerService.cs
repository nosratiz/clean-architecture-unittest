using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Hastnama.Solico.Common.Helper.Claims.Customer
{
    public class CurrentCustomerService : ICurrentCustomerService
    {
        public CurrentCustomerService(IHttpContextAccessor httpContextAccessor)
        {
            Id = httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
            FullName = httpContextAccessor.HttpContext?.User?.FindFirstValue("fullName");
            Mobile = httpContextAccessor.HttpContext?.User?.FindFirstValue("Mobile");
            CustomerId = httpContextAccessor.HttpContext?.User?.FindFirstValue("CustomerId");
        }

        public string FullName { get; set; }
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Mobile { get; set; }
    }
}
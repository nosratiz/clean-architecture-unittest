using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.User
{
    [Area("User")]
    [Route("[Area]/[Controller]")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class UserBaseController : Controller
    {
        
    }
}
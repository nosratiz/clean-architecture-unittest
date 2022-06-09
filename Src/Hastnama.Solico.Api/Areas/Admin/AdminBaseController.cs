using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    [Area("Admin")]
    [Route("[Area]/[Controller]")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class AdminBaseController : Controller
    {
    }
}

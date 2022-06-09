using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    [Route("[Controller]")]
    [EnableCors]
    [ApiController]

    public class BaseController : Controller
    {

    }
}

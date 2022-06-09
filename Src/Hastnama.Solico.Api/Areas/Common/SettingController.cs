using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Settings.Command.UpdateSetting;
using Hastnama.Solico.Application.Cms.Settings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class SettingController : BaseController
    {
        private readonly IMediator _mediator;

        public SettingController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var setting = await _mediator.Send(new GetSettingQuery(), cancellationToken);

            return setting.ApiResult;
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateSettingCommand updateSettingCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateSettingCommand, cancellationToken);

            if (result.Success==false)
                return result.ApiResult;

            return NoContent();
        }
    }
}

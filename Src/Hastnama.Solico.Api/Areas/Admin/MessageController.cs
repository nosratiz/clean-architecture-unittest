using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Messages.Command.DeleteUserMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyUserMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage;
using Hastnama.Solico.Application.Cms.Messages.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class MessageController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAdminUserMessageQuery model,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(model, cancellationToken));


        [HttpGet("{messageId}/conversation")]
        public async Task<IActionResult> GetMessage(Guid messageId, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetConversationQuery { MessageId = messageId }, cancellationToken));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteUserMessageCommand { Id = id }, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateMessage(SendCustomerMessageCommand sendCustomerMessageCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(sendCustomerMessageCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost("ReplyMessage")]
        public async Task<IActionResult> ReplyMessage(ReplyUserMessageCommand replyUserMessageCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(replyUserMessageCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
        
        
        
    }
}
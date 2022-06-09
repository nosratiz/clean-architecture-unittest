using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Messages.Command.DeleteCustomerMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyCustomerMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage;
using Hastnama.Solico.Application.Cms.Messages.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.User
{
    public class MessageController : UserBaseController
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetCustomerMessageQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Search = pagingOptions.Search
            }, cancellationToken));
        
        [HttpGet("{messageId}/conversation")]
        public async Task<IActionResult> GetMessage(Guid messageId, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetConversationQuery { MessageId = messageId }, cancellationToken));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCustomerMessageCommand { Id = id }, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateMessage(SendUserMessageCommand sendUserMessageCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(sendUserMessageCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost("ReplyMessage")]
        public async Task<IActionResult> ReplyMessage(ReplyCustomerMessageCommand replyCustomerMessageCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(replyCustomerMessageCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}
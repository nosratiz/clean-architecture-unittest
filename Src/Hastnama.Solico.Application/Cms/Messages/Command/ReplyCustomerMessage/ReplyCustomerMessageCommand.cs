using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Messages.Command.ReplyCustomerMessage
{
    public class ReplyCustomerMessageCommand : IRequest<Result>
    {
        public Guid ParentMessageId { get; set; }
        
        public string Content { get; set; }
        
        public string File { get; set; }
    }
}
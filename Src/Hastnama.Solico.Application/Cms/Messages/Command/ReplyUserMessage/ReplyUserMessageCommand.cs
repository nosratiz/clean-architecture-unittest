using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Messages.Command.ReplyUserMessage
{
    public class ReplyUserMessageCommand : IRequest<Result>
    {
        public Guid ParentMessageId { get; set; }
        
        public string Content { get; set; }
        
        public string File { get; set; }
    }
}
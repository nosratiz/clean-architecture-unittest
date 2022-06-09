using System;
using Hastnama.Solico.Application.Cms.Messages.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage
{
    public class SendCustomerMessageCommand : IRequest<Result>
    {
        public Guid CustomerId { get; set; }
       
        public string Title { get; set; }
        
        public string Content { get; set; }

        public string File { get; set; }
    }
}
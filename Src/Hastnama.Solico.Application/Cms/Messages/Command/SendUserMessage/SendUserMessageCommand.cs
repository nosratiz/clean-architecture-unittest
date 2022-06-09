using Hastnama.Solico.Common.Result;
using MediatR;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage
{
    public class SendUserMessageCommand : IRequest<Result>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string File { get; set; }
    }
}
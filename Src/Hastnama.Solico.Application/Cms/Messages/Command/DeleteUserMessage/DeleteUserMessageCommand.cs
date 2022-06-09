using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Messages.Command.DeleteUserMessage
{
    public class DeleteUserMessageCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
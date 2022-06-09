using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Messages.Command.DeleteCustomerMessage
{
    public class DeleteCustomerMessageCommand  : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
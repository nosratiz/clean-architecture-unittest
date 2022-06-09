using System.Collections.Generic;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Subscribers.Command.Delete
{
    public class DeleteSubscriberCommand : IRequest<Result>
    {
        public List<long> Ids { get; set; }
    }
}
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Subscribers.Command.Create
{
    public class CreateSubscriberCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }
}
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.ContactUses.Command.Create
{
    public class CreateContactUsCommand : IRequest<Result>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}
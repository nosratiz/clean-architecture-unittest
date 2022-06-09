using System.Collections.Generic;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.ContactUses.Command.Delete
{
    public class DeleteContactUsCommand : IRequest<Result>
    {
        public List<int> Ids { get; set; }
    }
}
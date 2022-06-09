using System.Collections.Generic;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Faqs.Command.Delete
{
    public class DeleteFaqCommand : IRequest<Result>
    {
        public List<int> Ids { get; set; }
    }
}
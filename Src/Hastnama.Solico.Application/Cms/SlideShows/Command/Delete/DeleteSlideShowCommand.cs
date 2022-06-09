using System.Collections.Generic;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.SlideShows.Command.Delete
{
    public class DeleteSlideShowCommand : IRequest<Result>
    {
        public List<int> Ids { get; set; }
    }
}
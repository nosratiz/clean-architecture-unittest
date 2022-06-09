using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.HtmlParts.Command.Update
{
    public class UpdateHtmlPartCommand : IRequest<Result>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}
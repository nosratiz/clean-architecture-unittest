using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.SlideShows.Command.Update
{
    public class UpdateSlideShowCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }

        public string Url { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public bool IsVisible { get; set; }
    }
}
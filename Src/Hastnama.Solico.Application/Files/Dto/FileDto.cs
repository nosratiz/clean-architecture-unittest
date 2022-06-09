using System;

namespace Hastnama.Solico.Application.Files.Dto
{
    public class FileDto
    {
        public long Id { get; set; }
        public double Size { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string MediaType { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
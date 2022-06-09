using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Settings.Command.UpdateSetting
{
    public class UpdateSettingCommand : IRequest<Result>
    {
        public string Image { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MerchantId { get; set; }
        public string DefaultLanguageCode { get; set; }
        public string ENamad { get; set; }
        public string Logo { get; set; }
        public string CopyRight { get; set; }
        public string GoogleAnalytic { get; set; }
        public string GoogleMaster { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Email { get; set; }
        public string SocialMedia { get; set; }
        public string Privacy { get; set; }
        public string Terms { get; set; }
        public long UserId { get; set; }
        public string TerminalId { get; set; }

        public int MaxSizeUploadFile { get; set; }
        public int MaxSlideShow { get; set; }

        public bool MaintenanceMode { get; set; }

    }
}
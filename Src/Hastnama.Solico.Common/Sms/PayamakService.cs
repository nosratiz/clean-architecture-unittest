using System.Threading.Tasks;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Common.RequestStrategy;
using Hastnama.Solico.Common.TemplateNotification;
using Microsoft.Extensions.Options;

namespace Hastnama.Solico.Common.Sms
{
    public class PayamakService : IPayamakService
    {
        private readonly INotificationTemplateGenerator _notificationTemplateGenerator;
        private readonly IOptionsMonitor<Payamak> _payamak;


        public PayamakService(INotificationTemplateGenerator notificationTemplateGenerator,
            IOptionsMonitor<Payamak> payamakService)
        {
            _notificationTemplateGenerator = notificationTemplateGenerator;
            _payamak = payamakService;
        }

        public async Task SendMessage(string receptor, string confirmCode)
        {
            RequestStrategyContext Context = new RequestStrategyContext();

            Context.DetectStrategy(HttpMethod.Get);

            var text = _notificationTemplateGenerator.CreateConfirmCode(new ConfirmCodeTemplate {Code = confirmCode});

            await Context.ExecuteRequest(
                $"{_payamak.CurrentValue.BaseUrl}text={text}&destinationNumbers={receptor}&applicationID={_payamak.CurrentValue.ApplicationId}&sourceNumber={_payamak.CurrentValue.SourceNumber}",
                "");
        }
    }
}
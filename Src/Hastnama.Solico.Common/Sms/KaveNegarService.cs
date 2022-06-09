using System;
using System.Threading.Tasks;
using Hastnama.Solico.Common.Options;
using Kavenegar;
using Kavenegar.Core.Models;
using Microsoft.Extensions.Options;

namespace Hastnama.Solico.Common.Sms
{
    public class KaveNegarService : IKaveNegarService
    {
        private readonly IOptionsMonitor<Payamak> _smsPanelOptions;

        public KaveNegarService(IOptionsMonitor<Payamak> smsPanelOptions)
        {
            _smsPanelOptions = smsPanelOptions;
        }

        public async Task<SendResult> SendConfirmedCode(string phoneNumber, string code)
        {
            try
            {
                var api = new KavenegarApi(_smsPanelOptions.CurrentValue.ApiKey);

                return await api.VerifyLookup(phoneNumber, code, "SolicoLogin");
            }
            catch (Exception ex)
            {
           
                return new SendResult {Message = ex.Message};
            }
        }
        
    }
}
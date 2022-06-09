using System.Threading.Tasks;
using Kavenegar.Core.Models;

namespace Hastnama.Solico.Common.Sms
{
    public interface IKaveNegarService
    {
        Task<SendResult> SendConfirmedCode(string phoneNumber, string code);
        

    }
}
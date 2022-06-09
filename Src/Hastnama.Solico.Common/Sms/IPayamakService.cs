using System.Threading.Tasks;

namespace Hastnama.Solico.Common.Sms
{
    public interface IPayamakService
    {
        Task SendMessage(string receptor, string confirmCode);
    }
}
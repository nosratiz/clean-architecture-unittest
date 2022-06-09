using System.Threading.Tasks;

namespace Hastnama.Solico.Common.Email
{
    public interface IEmailServices
    {
        Task SendMessage(string emailTo, string subject, string body);
    }
}
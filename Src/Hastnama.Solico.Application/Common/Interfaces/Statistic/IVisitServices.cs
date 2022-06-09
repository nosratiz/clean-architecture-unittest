using System.Threading.Tasks;

namespace Hastnama.Solico.Application.Common.Interfaces.Statistic
{
    public interface IVisitServices
    {
        Task<bool> HasVisitRecently( string slug);
    }
}
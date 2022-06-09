using System.Threading;
using System.Threading.Tasks;

namespace Hastnama.Solico.Application.Common.Interfaces.Statistic
{
    public interface IDailyStatistic
    {
        Task UpdateRegisterUser(CancellationToken cancellationToken);
        Task UpdateOrder(CancellationToken cancellationToken);
        Task UpdateRevenue(int revenue,CancellationToken cancellationToken);
        Task UpdateView(CancellationToken cancellationToken);

    }
}
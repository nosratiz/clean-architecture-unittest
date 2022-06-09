using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Domain.Models.Logs;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface ISolicoEventService
    {
        Task AddEvent(SolicoEventLog solicoEventLog,CancellationToken cancellationToken);
    }
}
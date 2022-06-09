using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Domain.Models.Logs;

namespace Hastnama.Solico.Application.Common.Services
{
    public class SolicoEventService : ISolicoEventService
    {
        private readonly ISolicoDbContext _context;

        public SolicoEventService(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task AddEvent(SolicoEventLog solicoEventLog,CancellationToken cancellationToken)
        {
            await _context.SolicoEventLogs.AddAsync(solicoEventLog,cancellationToken);

            await _context.SaveAsync(cancellationToken);
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using MediatR;

namespace Hastnama.Solico.Application.System
{
    public class SampleSeedDataCommand : IRequest
    {
    }

    public class SampleSeedDataCommandHandler :IRequestHandler<SampleSeedDataCommand>
    {
        private readonly ISolicoDbContext _context;

        public SampleSeedDataCommandHandler(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SampleSeedDataCommand request, CancellationToken cancellationToken)
        {
           var seeder=new SeedData(_context);

           await seeder.SeedAllAsync(cancellationToken);

           return Unit.Value;
        }
    }
}

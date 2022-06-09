using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Subscribers.Command.Create
{
    public class CreateSubscriberCommandHandler : IRequestHandler<CreateSubscriberCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public CreateSubscriberCommandHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = _mapper.Map<Subscribe>(request);

            await _context.Subscribes.AddAsync(subscriber, cancellationToken);
            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.ContactUses.Command.Create
{
    public class CreateContactUsCommandHandler : IRequestHandler<CreateContactUsCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;


        public CreateContactUsCommandHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateContactUsCommand request, CancellationToken cancellationToken)
        {
            var contactUs = _mapper.Map<ContactUs>(request);

            await _context.ContactUs.AddAsync(contactUs, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}
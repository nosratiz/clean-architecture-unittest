using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Settings.Command.UpdateSetting
{
    public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand,Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public UpdateSettingCommandHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
        {
            var setting = await _context.AppSettings.FirstOrDefaultAsync(cancellationToken);

            _mapper.Map(request, setting);

            await _context.SaveAsync(cancellationToken);
            
            return Result.SuccessFul();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Settings.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Settings.Queries
{
    public class GetSettingQuery : IRequest<Result<AppSettingDto>>
    {

    }

    public class GetSettingQueryHandler : IRequestHandler<GetSettingQuery, Result<AppSettingDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetSettingQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<AppSettingDto>> Handle(GetSettingQuery request, CancellationToken cancellationToken)
        {
            var setting = await _context.AppSettings
                .Include(x=>x.User)
                .FirstOrDefaultAsync(cancellationToken);

            return Result<AppSettingDto>.SuccessFul(_mapper.Map<AppSettingDto>(setting));
        }
    }
}
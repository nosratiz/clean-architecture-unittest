using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Statistics.Dto;
using Hastnama.Solico.Domain.Models.Statistic;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Statistics.Queires
{
    public class GetDailyStatisticQuery : IRequest<List<DailyStatisticDto>>
    {
        public DateTime? FromDate { get; set; } = DateTime.Now.AddDays(-7);

        public DateTime? ToDate { get; set; } = DateTime.Now;
    }

    public class GetDailyStatisticQueryHandler : IRequestHandler<GetDailyStatisticQuery, List<DailyStatisticDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetDailyStatisticQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DailyStatisticDto>> Handle(GetDailyStatisticQuery request, CancellationToken cancellationToken)
        {
            request.FromDate ??= DateTime.Now.AddDays(-7);
            request.ToDate ??= DateTime.Now;

            List<DailyStatistic> dailyStatistics = new List<DailyStatistic>();

            var stat = await _context.DailyStatistics
                .Where(x => x.Date >= request.FromDate && x.Date <= request.ToDate)
                .ToListAsync(cancellationToken);

            for (int i = 0; i < 7; i++)
            {
                var statistic = stat.FirstOrDefault(x => x.Date.Date == DateTime.Now.AddDays(-i).Date);

                if (statistic is null)
                    dailyStatistics.Add(new DailyStatistic
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(-i),
                        TotalOrder = 0,
                        TotalRevenue = 0,
                        TotalView = 0,
                        RegisterUser = 0
                    });

                else
                    dailyStatistics.Add(statistic);
            }

            return _mapper.Map<List<DailyStatisticDto>>(dailyStatistics);
        }
    }
}
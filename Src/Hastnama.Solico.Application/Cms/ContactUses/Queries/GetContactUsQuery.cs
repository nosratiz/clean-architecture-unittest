using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.ContactUses.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.ContactUses.Queries
{
    public class GetContactUsQuery : IRequest<Result<ContactUsDto>>
    {
        public int Id { get; set; }
    }

    public class GetContactUsQueryHandler : IRequestHandler<GetContactUsQuery, Result<ContactUsDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetContactUsQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<ContactUsDto>> Handle(GetContactUsQuery request, CancellationToken cancellationToken)
        {
            var contactUs = await _context.ContactUs.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (contactUs is null)
                return Result<ContactUsDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ContentNotFound,
                        cancellationToken))));


            return Result<ContactUsDto>.SuccessFul(_mapper.Map<ContactUsDto>(contactUs));
        }
    }
}
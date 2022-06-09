using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.CustomerConsults.Command.Create
{
    public class CreateCustomerConsultCommandHandler : IRequestHandler<CreateCustomerConsultCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public CreateCustomerConsultCommandHandler(ISolicoDbContext context,
            ICurrentCustomerService currentCustomerService, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _currentCustomerService = currentCustomerService;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(CreateCustomerConsultCommand request, CancellationToken cancellationToken)
        {
            if (await HasAlreadyRegisterForConsultAsync(cancellationToken))
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.CustomerHasAlreadyRegisterForConsult,
                        cancellationToken))));
            }


            var consult = _mapper.Map<CustomerConsult>(request);
            
            consult.CustomerId=Guid.Parse(_currentCustomerService.Id);

            await _context.CustomerConsults.AddAsync(consult,cancellationToken);
            
            await _context.SaveAsync(cancellationToken);
            
            return Result.SuccessFul();
        }

        #region Query

        private async Task<bool> HasAlreadyRegisterForConsultAsync(CancellationToken cancellationToken)
        {
            return await _context.CustomerConsults.AnyAsync(
                x => x.CustomerId == Guid.Parse(_currentCustomerService.Id) && x.IsSettle == false, cancellationToken);
        }

        #endregion
       
    }
}
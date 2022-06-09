using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.Create
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerEnrollmentDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<CustomerEnrollmentDto>> Handle(CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<CustomerEnrollment>(request);

            await _context.CustomerEnrollments.AddAsync(customer, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<CustomerEnrollmentDto>.SuccessFul(_mapper.Map<CustomerEnrollmentDto>(customer));
        }
    }
}
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.Create
{
    public class CreateCustomerCommand : IRequest<Result<CustomerEnrollmentDto>>
    {
        public string SolicoCustomerId { get; set; }
    }
}
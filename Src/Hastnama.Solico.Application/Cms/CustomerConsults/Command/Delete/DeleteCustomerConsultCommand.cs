using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.CustomerConsults.Command.Delete
{
    public class DeleteCustomerConsultCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
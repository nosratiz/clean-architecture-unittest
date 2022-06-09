using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Create;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Customers
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly ILocalization _localization;
        private readonly ISolicoDbContext _context;

        public CreateCustomerCommandValidator(ILocalization localization, ISolicoDbContext context)
        {
            _localization = localization;
            _context = context;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.SolicoCustomerId)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.SolicoCustomerIdIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.SolicoCustomerIdIsRequired))
                .MustAsync(HasCustomerExist)
                .WithMessage(_localization.GetMessage(ResponseMessage.CustomerAlreadyExist));
        }

        private async Task<bool> HasCustomerExist(string solicoCustomerId, CancellationToken cancellationToken)
        {
            return !await _context.Customers.AnyAsync(x => x.SolicoCustomerId == solicoCustomerId, cancellationToken) 
                   && !await _context.CustomerEnrollments.AnyAsync(x => x.SolicoCustomerId == solicoCustomerId,
                cancellationToken);
        }
    }
}
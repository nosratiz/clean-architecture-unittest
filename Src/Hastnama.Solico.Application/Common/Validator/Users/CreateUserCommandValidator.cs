using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Users.Command.CreateUser;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Users
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        public CreateUserCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            CascadeMode = CascadeMode.Stop;
            _context = context;
            _localization = localization;

            RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
            .EmailAddress().WithMessage(_localization.GetMessage(ResponseMessage.InvalidEmailFormat))
            .MustAsync(ValidEmail).WithMessage(_localization.GetMessage(ResponseMessage.EmailIsAlreadyExist));

            RuleFor(dto => dto.Family)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.FamilyIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.FamilyIsRequired));

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));

            RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.PasswordIsRequired));

            RuleFor(dto => dto.PhoneNumber)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired))
            .MustAsync(ValidMobile).WithMessage(_localization.GetMessage(ResponseMessage.MobileAlreadyExist))
            .Must(x=>x.Contains("09"));

            RuleFor(dto => dto.RoleId).NotEmpty().NotNull()
            .MustAsync(ValidRole).WithMessage(_localization.GetMessage(ResponseMessage.RoleNotFound));

        }

        private async Task<bool> ValidRole(int roleId, CancellationToken cancellation)
        {
            if (!await _context.Roles.AnyAsync(x => x.Id == roleId, cancellation))
                return false;

            return true;

        }

        private async Task<bool> ValidEmail(string email, CancellationToken cancellation)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email, cancellation))
                return false;

            return true;
        }

        private async Task<bool> ValidMobile(string mobile, CancellationToken cancellation)
        {
            if (await _context.Users.AnyAsync(x => x.PhoneNumber == mobile, cancellation))
                return false;

            return true;
        }
    }
}
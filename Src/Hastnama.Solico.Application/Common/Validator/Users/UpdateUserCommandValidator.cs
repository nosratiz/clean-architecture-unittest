using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateUser;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Common.Validator.Users
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        public UpdateUserCommandValidator(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;

            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Id).NotEmpty().NotNull();

            RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.EmailIsRequired))
            .EmailAddress().WithMessage(_localization.GetMessage(ResponseMessage.InvalidEmailFormat));

            RuleFor(dto => dto.Family)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.FamilyIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.FamilyIsRequired));

            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.NameIsRequired));


            RuleFor(dto => dto.PhoneNumber)
            .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired))
            .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.MobileIsRequired));


            RuleFor(dto => dto.RoleId).NotEmpty().NotNull()
            .MustAsync(ValidRole).WithMessage(_localization.GetMessage(ResponseMessage.RoleNotFound));

            RuleFor(dto => dto).MustAsync(validEmail)
            .WithMessage(_localization.GetMessage(ResponseMessage.EmailIsAlreadyExist))
            .MustAsync(ValidMobile).WithMessage(_localization.GetMessage(ResponseMessage.MobileAlreadyExist));

        }


        private async Task<bool> ValidRole(int roleId, CancellationToken cancellation)
        {
            if (!await _context.Roles.AnyAsync(x => x.Id == roleId, cancellation))
                return false;

            return true;
        }

        private async Task<bool> validEmail(UpdateUserCommand updateUser, CancellationToken cancellation)
        {
            if (await _context.Users.AnyAsync(x => x.Email == updateUser.Email && x.Id != updateUser.Id, cancellation))
                return false;

            return true;
        }

        private async Task<bool> ValidMobile(UpdateUserCommand updateUser, CancellationToken cancellation)
        {

            if (await _context.Users.AnyAsync(x => x.PhoneNumber == updateUser.PhoneNumber && x.Id != updateUser.Id, cancellation))
                return false;

            return true;
        }
    }
}
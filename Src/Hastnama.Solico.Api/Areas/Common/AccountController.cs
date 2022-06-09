using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Auth.Command.ChangePassword;
using Hastnama.Solico.Application.Auth.Command.ForgetPassword;
using Hastnama.Solico.Application.Auth.Command.Login;
using Hastnama.Solico.Application.Auth.Command.LogOut;
using Hastnama.Solico.Application.Auth.Command.ResetPassword;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ChangePassword;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Login;
using Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode;
using Hastnama.Solico.Application.UserManagement.Customers.Queries;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateProfile;
using Hastnama.Solico.Application.UserManagement.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(loginCommand, cancellationToken);

            return result.ApiResult;
        }

        [HttpPost("CustomerLogin")]
        public async Task<IActionResult> CustomerLogin(CustomerLoginCommand customerLoginCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(customerLoginCommand, cancellationToken);

            return result.ApiResult;
        }


        [HttpPost("SendConfirmCode")]
        public async Task<IActionResult> SendConfirmCode(SendConfirmCodeCommand sendConfirmCodeCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(sendConfirmCodeCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost("ConfirmCode")]
        public async Task<IActionResult> ConfirmCode(ConfirmCodeCommand confirmCodeCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(confirmCodeCommand, cancellationToken);

            return result.ApiResult;
        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand forgetPasswordCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(forgetPasswordCommand, cancellationToken);

            return result.ApiResult;
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(changePasswordCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand resetPasswordCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(resetPasswordCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand updateProfileCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateProfileCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> MyProfile(CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetProfileQuery(), cancellationToken));


        [HttpGet("MyCredit")]
        public async Task<IActionResult> MyCredit(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerCreditQuery(), cancellationToken);

            return result.ApiResult;
        }


        [HttpGet("CustomerProfile")]
        public async Task<IActionResult> GetCustomerProfile(CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetCustomerProfileQuery(), cancellationToken));


        [HttpPost("CustomerChangePassword")]
        public async Task<IActionResult> ChangeCustomerPassword(ChangePasswordCustomerCommand changePasswordCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(changePasswordCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpGet("Logout")]
        public async Task<IActionResult> LogOut(CancellationToken cancellationToken)
        {
            _ = await _mediator.Send(new LogOutCommand(), cancellationToken);

            return NoContent();
        }
    }
}
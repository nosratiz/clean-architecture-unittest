using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.UserManagement.Users.Command.CreateUser;
using Hastnama.Solico.Application.UserManagement.Users.Command.DeleteUser;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateUser;
using Hastnama.Solico.Application.UserManagement.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class UserController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUserListQuery getUserListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getUserListQuery, cancellationToken));


        [HttpGet("{id}", Name = "GetUserInfo")]
        public async Task<IActionResult> GetInfo(long id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserQuery {Id = id}, cancellationToken);

            return result.ApiResult;
        }


        [HttpPut("delete")]
        public async Task<IActionResult> Delete(DeleteUserCommand deleteUserCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteUserCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserCommand updateUserCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateUserCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand createUserCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createUserCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return CreatedAtAction(nameof(GetInfo), new {id=result.Data.Id}, result.Data);
        }
    }
}
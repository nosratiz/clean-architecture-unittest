using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Users.Command.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Result<User>>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Family { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Avatar { get; set; }
    }
}
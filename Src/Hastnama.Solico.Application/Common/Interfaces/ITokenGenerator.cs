using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        Task<AuthResult> Generate(User user, CancellationToken cancellationToken);

        Task<AuthResult> GenerateCustomerToken(Customer customer, CancellationToken cancellationToken);
    }
}
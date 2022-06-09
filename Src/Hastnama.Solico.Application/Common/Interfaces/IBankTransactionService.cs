using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Payments.Dto;
using Hastnama.Solico.Common.Result;
using Order = Hastnama.Solico.Domain.Models.Shop.Order;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface IBankTransactionService
    {
        Task<Result<string>> Purchase(Order order,CancellationToken cancellationToken);

        Task<Result> VerifyPayment(string token,long orderIndex,long amount,CancellationToken cancellationToken);
    }
}   
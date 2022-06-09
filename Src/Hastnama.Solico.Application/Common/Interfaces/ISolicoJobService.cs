using System.Threading;
using System.Threading.Tasks;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface ISolicoJobService
    {
        Task SyncCustomerAsync();
        Task SyncMaterialAsync();

        Task SyncNewCustomer();
        Task SyncCustomerPriceAsync();
        Task TrackSailedOrderAsync();

        Task SyncCustomerList();

        Task TrackProformaAsync();

        Task SyncCreditAsync();

        Task SyncQuotation();

        Task SyncOpenItems();

        Task SyncDeliverOrderAsync();

        Task DeleteExpiredUserTokenAsync();
    }
}
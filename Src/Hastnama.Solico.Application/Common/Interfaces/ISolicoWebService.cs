using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface ISolicoWebService
    {
        Task<List<SolicoMaterialDto>> GetMaterialListServiceAsync(CancellationToken cancellationToken);

        Task<string> FetchTokenAsync(CancellationToken cancellationToken);

        Task<List<SolicoCustomerDto>> GetAllCustomerAsync(CancellationToken cancellationToken);

        Task<List<SolicoCustomerDto>> GetCustomerAsync(List<SolicoPricingDto> KUNNR, CancellationToken cancellationToken);

        Task<List<SolicoListingServiceDto>> GetListServiceAsync(List<SolicoPricingDto> KUNNR, CancellationToken cancellationToken);

        Task<List<SolicoListingServiceDto>> GetPriceServiceAsync(List<SolicoPricingDto> KUNNR, CancellationToken cancellationToken);

        Task<CreditDto> GetCreditServiceAsync(List<SolicoPricingDto> KUNNR, CancellationToken cancellationToken);

        Task<List<DocumentFlowDto>> GetDocumentFlowServiceAsync(List<SolicoOrderNumberDto> VBELN, CancellationToken cancellationToken);

        Task<List<QuotationSalesOrderDto>> GetQuotationSalesOrderServiceAsync(List<SolicoOrderNumberDto> VBELN, CancellationToken cancellationToken);

        Task<List<SolicoQuotationProformaDto>> GetSolicoQuotationProformaServiceAsync(List<SolicoOrderNumberDto> VBELN, CancellationToken cancellationToken);

        Task<SolicoOpenItemDto> GetOpenItemServiceAsync(CreateOpenItemDto createQuotationDto, CancellationToken cancellationToken);

        Task<QuotationResponse> CreateQuotation(SolicoQuotationDto solicoQuotationDto,string token, CancellationToken cancellationToken);
     
    }
}
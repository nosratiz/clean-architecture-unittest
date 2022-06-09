using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Domain.Models.Logs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Hastnama.Solico.Application.Common.Services
{
    public class SolicoWebService : ISolicoWebService
    {
        private readonly ISolicoEventService _solicoEventService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsMonitor<SolicoWebServiceSetting> _solicoWebServiceSetting;

        public SolicoWebService(IOptionsMonitor<SolicoWebServiceSetting> solicoWebServiceSetting,
            ISolicoEventService solicoEventService, IHttpClientFactory httpClientFactory)
        {
            _solicoWebServiceSetting = solicoWebServiceSetting;
            _solicoEventService = solicoEventService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<SolicoMaterialDto>> GetMaterialListServiceAsync(CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var response = await client.GetAsync("/zsalesportal/sp_material?sap-client=100", cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);


            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_material?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<List<SolicoMaterialDto>>(responseBody);
        }

        public async Task<string> FetchTokenAsync(CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/sp_fetchtoken?sap-client=100")
            {
            };

            request.Headers.Add("x-csrf-Token", "fetch");

            var response = await client.SendAsync(request, cancellationToken);


            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_fetchtoken?sap-client=100",
                Content = response.Content.ToString(),
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                return string.Empty;
            }


            return response.Headers.GetValues("x-csrf-token").FirstOrDefault();
        }

        #region customer

        public async Task<List<SolicoCustomerDto>> GetAllCustomerAsync(CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var response = await client.GetAsync("/zsalesportal/sp_customer?sap-client=100", cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);


            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_customer?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<List<SolicoCustomerDto>>(responseBody);
        }

        public async Task<List<SolicoCustomerDto>> GetCustomerAsync(List<SolicoPricingDto> KUNNR,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Post,
                "/zsalesportal/sp_customer?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(KUNNR), Encoding.UTF8, "application/json")
            };


            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_customer?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            
            return JsonSerializer.Deserialize<List<SolicoCustomerDto>>(responseBody);
        }

        #endregion

        #region Price Service

        public async Task<List<SolicoListingServiceDto>> GetListServiceAsync(List<SolicoPricingDto> KUNNR,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/sp_listing?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(KUNNR), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_listing?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<List<SolicoListingServiceDto>>(responseBody);
        }

        public async Task<List<SolicoListingServiceDto>> GetPriceServiceAsync(List<SolicoPricingDto> KUNNR,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/sp_pricing")
            {
                Content = new StringContent(JsonSerializer.Serialize(KUNNR), Encoding.UTF8, "application/json")
            };


            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_pricing",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<List<SolicoListingServiceDto>>(responseBody);
        }

        #endregion


        public async Task<CreditDto> GetCreditServiceAsync(List<SolicoPricingDto> KUNNR,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/credit_control?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(KUNNR), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);


            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/credit_control?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<CreditDto>(responseBody);
        }

        public async Task<List<DocumentFlowDto>> GetDocumentFlowServiceAsync(List<SolicoOrderNumberDto> VBELN,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/document_flow?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(VBELN), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/document_flow?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<List<DocumentFlowDto>>(responseBody);
        }

        public async Task<List<QuotationSalesOrderDto>> GetQuotationSalesOrderServiceAsync(
            List<SolicoOrderNumberDto> VBELN,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get, "/zsalesportal/quotation_salesorder?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(VBELN), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url =
                    $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/quotation_salesorder?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<List<QuotationSalesOrderDto>>(responseBody);
        }

        public async Task<List<SolicoQuotationProformaDto>> GetSolicoQuotationProformaServiceAsync(
            List<SolicoOrderNumberDto> VBELN,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/quotation_proforma?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(VBELN), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/quotation_proforma?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }


            return JsonSerializer.Deserialize<List<SolicoQuotationProformaDto>>(responseBody);
        }

        public async Task<SolicoOpenItemDto> GetOpenItemServiceAsync(CreateOpenItemDto createQuotationDto,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            var request = new HttpRequestMessage(HttpMethod.Get,
                "/zsalesportal/openitem_list?sap-client=100")
            {
                Content = new StringContent(JsonSerializer.Serialize(createQuotationDto), Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/openitem_list?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<SolicoOpenItemDto>(responseBody);
        }

        public async Task<QuotationResponse> CreateQuotation(SolicoQuotationDto solicoQuotationDto, string token,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Solico");

            client.DefaultRequestHeaders.Add("x-csrf-Token", token);

            var response = await client.PostAsync("/zsalesportal/sp_quotation?sap-client=100", new StringContent(
                JsonSerializer.Serialize(solicoQuotationDto), Encoding.UTF8,
                "application/json"), cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            await _solicoEventService.AddEvent(new SolicoEventLog
            {
                Url = $"{_solicoWebServiceSetting.CurrentValue.BaseUrl}/zsalesportal/sp_quotation?sap-client=100",
                Content = responseBody,
                IsSuccess = response.IsSuccessStatusCode,
                CreateDate = DateTime.Now
            }, cancellationToken);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"The solico web service sent back {response.StatusCode} Error Code");
            }

            return JsonSerializer.Deserialize<QuotationResponse>(responseBody);
        }
    }
}
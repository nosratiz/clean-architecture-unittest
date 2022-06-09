using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Payments.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NETCore.Encrypt;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Order = Hastnama.Solico.Domain.Models.Shop.Order;

namespace Hastnama.Solico.Application.Common.Services
{
    public class BankTransactionService : IBankTransactionService
    {
        private readonly IOptionsMonitor<HostAddress> _hostAddress;
        private readonly ISolicoDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalization _localization;
        private const string SuccessPayment ="0";

        public BankTransactionService(IOptionsMonitor<HostAddress> hostAddress, ISolicoDbContext context,
            IHttpClientFactory httpClientFactory, ILocalization localization)
        {
            _hostAddress = hostAddress;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _localization = localization;
        }

        public async Task<Result<string>> Purchase(Order order, CancellationToken cancellationToken)
        {
            var setting = await _context.AppSettings.FirstOrDefaultAsync(cancellationToken);

            var client = _httpClientFactory.CreateClient();


            var response = await client.PostAsync(_hostAddress.CurrentValue.PaymentUrl, new StringContent(
                JsonSerializer.Serialize(new SadadPurchaseDto
                {
                    MerchantId = setting.MerchantId,
                    TerminalId = setting.TerminalId,
                    Amount = Convert.ToInt64(order.FinalAmount),
                    LocalDateTime = DateTime.Now,
                    ReturnUrl = _hostAddress.CurrentValue.CallBackUrl,
                    SignData = EncryptProvider.Base64Encrypt(
                        $"{setting.TerminalId};{order.OrderIndex};{Convert.ToInt64(order.FinalAmount)}"),
                    OrderId = order.OrderIndex
                }), Encoding.UTF8,
                "application/json"), cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                return Result<string>.Failed(await _localization.GetMessage(ResponseMessage.UnknownErrorPortal, cancellationToken));
            }

            var sadadResponse = JsonSerializer.Deserialize<SadadResponsePurchaseDto>(responseBody);

            if (sadadResponse!.ResCode != SuccessPayment)
            {
                var sadadErrorResponse =
                    PortalResponseCode.Response.TryGetValue(int.Parse(sadadResponse.ResCode), out var error);

                if (sadadErrorResponse == false)
                {
                    return Result<string>.Failed(error);
                }

                return Result<string>.Failed(
                        await _localization.GetMessage(ResponseMessage.UnknownErrorPortal, cancellationToken));
            }


            await _context.BankTransactions.AddAsync(new BankTransaction
            {
                CreateDate = DateTime.Now,
                OrderId = order.Id,
                Price = Convert.ToInt64(order.FinalAmount),
                Token = sadadResponse!.Token,
                Status = 100
            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<string>.SuccessFul($"{_hostAddress.CurrentValue.RedirectUrl}?token={sadadResponse.Token}");
        }


        public async Task<Result> VerifyPayment(string token, long orderIndex, long amount,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();

            var setting = await _context.AppSettings.FirstOrDefaultAsync(cancellationToken);

            var response = await client.PostAsync(_hostAddress.CurrentValue.VerifyUrl, new StringContent(
                JsonSerializer.Serialize(new VerifySadadDto()
                {
                    SignData = EncryptProvider.Base64Encrypt($"{setting.TerminalId};{orderIndex};{amount}"),
                    Token = token
                }), Encoding.UTF8,
                "application/json"), cancellationToken);
            
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);


            if (response.IsSuccessStatusCode == false)
            {
                return Result.Failed(await _localization.GetMessage(ResponseMessage.UnknownErrorPortal, cancellationToken));
            }

            var sadadResponse = JsonSerializer.Deserialize<SadadResponsePurchaseDto>(responseBody);

            if (sadadResponse!.ResCode != SuccessPayment)
            {
                var sadadErrorResponse =
                    PortalResponseCode.Response.TryGetValue(int.Parse(sadadResponse.ResCode), out var error);

                if (sadadErrorResponse == false)
                {
                    return Result.Failed(error);
                }

                return Result.Failed(await _localization.GetMessage(ResponseMessage.UnknownErrorPortal, cancellationToken));
            }

            return Result.SuccessFul();
        }
    }
}
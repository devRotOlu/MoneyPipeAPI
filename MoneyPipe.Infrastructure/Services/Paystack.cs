using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;
using MoneyPipe.Infrastructure.Services.Models;
using MoneyPipe.Infrastructure.Services.Models.Response;

namespace MoneyPipe.Infrastructure.Services
{
   public class Paystack(IOptions<PaystackOptions> options,IHttpClientFactory httpClientFactory):
   IVirtualAccountProvisioner
   {  
      private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
      private readonly PaystackOptions _paystackoptions = options.Value;

      public async Task<VirtualAccountResponse> CreateVirtualAccountAsync(VirtualAccountId virtualAccountId, string email)
      {
         var requestBody = new
         {
            customer = email,
            preferred_bank = "wema-bank",//"wema-bank", // optional, fetch dynamically if needed
            description = $"Virtual Account {virtualAccountId.Value}",
            currency = "NGN"
         };

         var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_paystackoptions.BaseUrl}/dedicated_account")
         {
            Content = JsonContent.Create(requestBody)
         };

         httpRequest.Headers.Add("Authorization", $"Bearer {_paystackoptions.SecretKey}");

         var response = await _httpClient.SendAsync(httpRequest);
         response.EnsureSuccessStatusCode();

         var payload = await response.Content.ReadFromJsonAsync<PaystackVirtualAccountResponse>();

         return new VirtualAccountResponse(payload!.Data.AccountId, payload.Data.AccountNumber,
         payload.Data.BankName,payload.Data.Currency, VirtualAccountMethod.Paystack.ToString());
      }
    }
}
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Options;
using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;
using MoneyPipe.Infrastructure.Services.Models;
using MoneyPipe.Infrastructure.Services.Models.Response;

namespace MoneyPipe.Infrastructure.Services
{
    public class Monnify : IVirtualAccountProvisioner
    {
        private readonly HttpClient _httpClient;
        private readonly MonnifyOptions _options;

        public Monnify(IHttpClientFactory httpClientFactory,
            IOptions<MonnifyOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _options = options.Value;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var authString = $"{_options.ApiKey}:{_options.SecretKey}";
            var base64Auth = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(authString));

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl}/api/v1/auth/login");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", base64Auth);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<MonnifyAuthResponse>();

            return payload!.ResponseBody.AccessToken;
        }


        public async Task<VirtualAccountResponse> CreateVirtualAccountAsync(VirtualAccountId virtualAccountId, string email)
        {
            var accessToken = await GetAccessTokenAsync();

            List<string> preferredBanks = ["50515"];

            var requestBody = new
            {
                accountReference = virtualAccountId.Value.ToString(),
                accountName = $"MoneyPipe / {email}",
                currencyCode = "NGN",
                contractCode = _options.ContractCode,
                customerEmail = email,
                customerName = "John Doe",
                bvn = "21212121212",
                getAllAvailableBanks = "true",
                preferredBanks
            };

             using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl}/api/v2/bank-transfer/reserved-accounts")
            {
                Content = JsonContent.Create(requestBody)
            };

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MonnifyVirtualAccountResponse>();

            var account = result!.ResponseBody.Accounts[0];

            return new VirtualAccountResponse("",account.AccountNumber,account.BankName,"NGN",VirtualAccountMethod.Monnify.ToString());
        }
    }
}
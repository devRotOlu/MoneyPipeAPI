using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;
using MoneyPipe.Infrastructure.Services.Models;
using MoneyPipe.Infrastructure.Services.Models.Response;

namespace MoneyPipe.Infrastructure.Services
{
    class Korapay : IVirtualAccountProvisioner
    {
        private readonly HttpClient _httpClient;
        private readonly KorapayOptions _options;

        public Korapay(IHttpClientFactory httpClientFactory,
            IOptions<KorapayOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _options = options.Value;
        }

        public async Task<VirtualAccountResponse> CreateVirtualAccountAsync(VirtualAccountId virtualAccountId, string email)
        {
            var request = new 
            {
                account_name = "John Doe",
                account_reference = virtualAccountId.Value.ToString(),
                permanent = true,
                bank_code = "000", // sandbox
                customer = new 
                {
                    name = "John Mark",
                    email 
                }
                ,
                kyc = new
                {
                    //type = "individual",
                    bvn = "11111111111"
                }
            };

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl}/merchant/api/v1/virtual-bank-account"
            );

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.SecretKey);

            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(httpRequest);
            var body = await response.Content.ReadAsStringAsync();


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"{response.StatusCode} : {body}");
            }
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<KorapayVirtualAccountResponse>();

            var data = content!.Data;

            return new VirtualAccountResponse(data.Unique_Id,data.Account_Number,data.Bank_Name,"NGN",VirtualAccountMethod.Korapay.ToString());
        }
    }
}
using System.Net.Http.Headers;
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
    public class FlutterWave : IVirtualAccountProvisioner, IPaymentCreationProvisioner
    {
        private readonly HttpClient _httpClient;
        private readonly FlutterWaveOptions _options;

        public FlutterWave(
            IHttpClientFactory httpClientFactory,
            IOptions<FlutterWaveOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _options = options.Value;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.ApiSecret);

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PaymentCreationResponse> CreatePayment(
            string paymentRef,
            decimal amount,
            string email,
            string currency,
            CancellationToken ct)
        {
            var requestBody = new
            {
                tx_ref = paymentRef,
                amount,
                currency,
                customer = new
                {
                    email
                }
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl}/v3/payments")
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<FlutterWavePaymentCreationResponse>(ct);

            return new PaymentCreationResponse(
                payload!.Data.Link,
                PaymentCreationMethod.FlutterWave.ToString());
        }

        public async Task<VirtualAccountResponse> CreateVirtualAccountAsync(
            VirtualAccountId virtualAccountId,
            string email)
        {
            var requestBody = new
            {
                email,
                reference = virtualAccountId.Value.ToString(),
                currency = "NGN",
                account_type = "static"
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_options.BaseUrl}/v3/virtual-account-numbers")
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content
                .ReadFromJsonAsync<FlutterWaveVirtualAccountResponse>();

            return new VirtualAccountResponse(
                payload!.Data.Id,
                payload.Data.Account_Number,
                payload.Data.Account_Bank_Name,
                "NGN",
                VirtualAccountMethod.FlutterWave.ToString());
        }
    }
}


using FlutterWave.Core;
using FlutterWave.Core.Models.Services.Foundations.FlutterWave.VirtualAccounts;
using Microsoft.Extensions.Configuration;
using MoneyPipe.Application.Enum;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Services.VirtualAccount
{
    public class FlutterWaveVirtualAccountProvisioner:IVirtualAccountProvisioner
    {
        private readonly FlutterWaveClient _flutterWaveClient;
        public FlutterWaveVirtualAccountProvisioner(IConfiguration configuration)
        {
            var config = new ApiConfigurations
            {
                ApiKey = configuration["Flutterwave:ApiSecret"]
            };
            _flutterWaveClient = new FlutterWaveClient(config);
        }

        public async Task<VirtualAccountResponse> CreateAsync(VirtualAccountId virtualAccountId, string email)
        {
            var _request = new CreateVirtualAccountRequest
            {
                IsPermanent = true,
                Narration = $"Virtual Account {virtualAccountId.Value}",
                //FirstName = "Olumide",
                Email = email,
                TxRef = virtualAccountId.Value.ToString(),
                Bvn = "12345678901",
                FirstName = "Test", 
                LastName = "User", 
                PhoneNumber = "2348012345678"
            };
 
            var requestBody = new CreateVirtualAccounts
            {
                Request = _request
            };

            var response = await _flutterWaveClient.VirtualAccounts.CreateVirtualAccountAsync(requestBody);

            if (response.Response.Status.ToLower() is not "success")
            {
                throw new Exception();
            }
            
            var accountDetails = response.Response.Data;
            return new VirtualAccountResponse(accountDetails.OrderRef,accountDetails.AccountNumber,
            accountDetails.BankName,"NGN", VirtualAccountMethod.FlutterWave.ToString());
        }
    }
}
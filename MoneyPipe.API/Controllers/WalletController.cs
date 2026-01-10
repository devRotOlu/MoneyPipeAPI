using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.DTOs.Responses;
using MoneyPipe.Application.Services.WalletManagement.Commands.AddVirtualAccount;
using MoneyPipe.Application.Services.WalletManagement.Commands.AddVirtualCard;
using MoneyPipe.Application.Services.WalletManagement.Commands.CreateWallet;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    public class WalletController(ISender mediatr,IMapper mapper) 
    : APIController
    {
        private readonly ISender _mediatr = mediatr;
        private readonly IMapper _mapper = mapper;

        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWallet([FromQuery] string currency)
        {
            var command = new CreateWalletCommand(currency);
            ErrorOr<WalletResult> result = await _mediatr.Send(command);

            return result.Match(
                wallet =>
                {
                    var dto = _mapper.Map<GetWalletDTO>(wallet);
                    return Ok(ApiResponse<GetWalletDTO>.Ok(dto,"Wallet Created!"));
                },
                Problem
            );
        }

        [HttpPost("add-virtual-account")]
        public async Task<IActionResult> AddVirtualAccount([FromQuery] Guid walletId,[FromQuery] string currency)
        {
            var command = new AddVirtualAccountCommand(walletId,currency);
            ErrorOr<Success> result = await _mediatr.Send(command);

            return result.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Your virtual account being processed. You'd receive notification.")),
                Problem
            );
        }

        [HttpPost("add-virtual-card")]
        public async Task<IActionResult> AddVirtualCard([FromQuery] Guid walletId,[FromQuery] string currency)
        {
            var command = new AddVirtualCardCommand(walletId,currency);
            ErrorOr<Success> result = await _mediatr.Send(command);
            return result.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Your virtual card being processed. You'd receive notification.")),
                Problem
            );
        }
    }
}
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.DTOs.Responses;
using MoneyPipe.Application.Services.Wallet.Commands.CreateWallet;

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
                error=> Problem(error)
            );
        }
    }
}
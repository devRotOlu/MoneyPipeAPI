using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.DTOs.Requests;
using MoneyPipe.API.DTOs.Responses;
using MoneyPipe.Application.Services.KYCManagement.Commands.AddDocument;
using MoneyPipe.Application.Services.KYCManagement.Commands.CompleteProfile;
using MoneyPipe.Application.Services.KYCManagement.Queries.GetKycStatus;

namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    public class KycController(ISender mediatr,IMapper mapper) 
    : APIController
    {
        private readonly IMapper _mapper = mapper;
        private readonly ISender _mediatr = mediatr;

        [HttpPost("profile/complete")]
        public async Task<IActionResult> CompleteProfile(CompleteProfileDTO dto)
        {
            var command = _mapper.Map<CompleteProfileCommand>(dto);
            ErrorOr<Success> profileResult = await _mediatr.Send(command); 

            return profileResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Document submitted successfully!"))
                ,
                Problem
            );
        }

        [HttpPost("document")]
        public async Task<IActionResult> AddDocument(AddDocumentDTO dto)
        {
            var command = _mapper.Map<AddDocumentCommand>(dto);
            ErrorOr<Success> docResult = await _mediatr.Send(command); 

            return docResult.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Document submitted successfully!"))
                ,
                Problem
            );
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetKycStatus()
        {
            var query = new GetKycStatusQuery();
            ErrorOr<GetKycSatusResult> statusResult = await _mediatr.Send(query);

            return statusResult.Match(
                value =>
                {
                    var response = _mapper.Map<GetKycStatusDTO>(value); 
                    return Ok(ApiResponse<GetKycStatusDTO>.Ok(response));
                },
                Problem
            );
        }


        // [HttpGet("get-invoices")]
        // public async Task<IActionResult> GetInvoices([FromQuery] int? pageSize,[FromQuery] DateTime? lastTimestamp)
        // {
        //     var command = new GetInvoicesQuery(pageSize,lastTimestamp);
        //     var Result = await _mediatr.Send(command);
        //     var dto = _mapper.Map<GetInvoicesDTO>(Result);
        //     return Ok(ApiResponse<GetInvoicesDTO>.Ok(dto));
        // }
    }
}
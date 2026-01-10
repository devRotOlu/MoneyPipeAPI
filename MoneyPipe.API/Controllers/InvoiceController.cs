using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.DTOs.Requests;
using MoneyPipe.API.DTOs.Responses;
using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.SendInvoice;
using MoneyPipe.Application.Services.Invoicing.Common;
using MoneyPipe.Application.Services.Invoicing.Queries.GetInvoice;
using MoneyPipe.Application.Services.Invoicing.Queries.GetInvoices;


namespace MoneyPipe.API.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController(ISender mediatr,IMapper mapper): APIController
    {
        private readonly ISender _mediatr = mediatr;
        private readonly IMapper _mapper = mapper;

        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDTO dto)
        {
            var command = _mapper.Map<CreateInvoiceCommand>(dto);
            ErrorOr<InvoiceResult> invoiceResult = await _mediatr.Send(command); 

            return invoiceResult.Match(
                invoice =>
                {
                    var dto = _mapper.Map<GetInvoiceDTO>(invoice);
                    return Ok(ApiResponse<GetInvoiceDTO>.Ok(dto,"Invoice Created Successfully!"));
                },
                Problem
            );
        }

        [HttpGet("get-invoices")]
        public async Task<IActionResult> GetInvoices([FromQuery] int? pageSize,[FromQuery] DateTime? lastTimestamp)
        {
            var command = new GetInvoicesQuery(pageSize,lastTimestamp);
            var invoiceResult = await _mediatr.Send(command);
            var dto = _mapper.Map<GetInvoicesDTO>(invoiceResult);
            return Ok(ApiResponse<GetInvoicesDTO>.Ok(dto));
        }

        [HttpGet("get-invoice")]
        public async Task<IActionResult> GetInvoice([FromQuery] Guid invoiceId)
        {
            var command = new GetInvoiceQuery(invoiceId);
            var invoiceResult = await _mediatr.Send(command);
            var dto = _mapper.Map<GetInvoiceDTO>(invoiceResult);
            return Ok(ApiResponse<GetInvoiceDTO>.Ok(dto));
        }

        [HttpPut("edit-invoice")]
        public async Task<IActionResult> EditInvoice(EditInvoiceDTO dto)
        {
            var command = _mapper.Map<EditInvoiceCommand>(dto);
            ErrorOr<InvoiceResult> invoiceResult = await _mediatr.Send(command);
            return invoiceResult.Match(
                invoice =>{
                    var dto = _mapper.Map<GetInvoiceDTO>(invoice);
                    return Ok(ApiResponse<GetInvoiceDTO>.Ok(dto, "Invoice successfully edited!"));
                },
                Problem
            );
        } 

        [HttpPost("send-invoice")]
        public async Task<IActionResult> SendInvoice(SendInvoiceDTO dto)
        {
            var command = _mapper.Map<SendInvoiceCommand>(dto);
            ErrorOr<Success> result = await _mediatr.Send(command);
            return result.Match(
                success => Ok(ApiResponse<object>.Ok(null,"Invoice Sent")),
                Problem
            );
        }  
    }
}
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyPipe.API.Common.Http;
using MoneyPipe.API.DTOs;
using MoneyPipe.Application.Services.Invoicing.Commands.Common;
using MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice;
using MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice;


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
                    var dto = _mapper.Map<GetInvoiceDTO>(invoiceResult);
                    return Ok(ApiResponse<GetInvoiceDTO>.Ok(dto,"Invoice Created Successfully!"));
                },
                Problem
            );
        }

        // [HttpGet("get-invoices")]
        // public async Task<IActionResult> GetInvoices()
        // {
        //     var invoices = await _invoiceService.GetInvoicesAsync(User);

        //     return Ok(ApiResponse<IEnumerable<GetInvoiceDTO>>.Ok(invoices));
        // }

        // [HttpGet("get-invoice")]
        // public async Task<IActionResult> GetInvoice([FromQuery] string invoiceId)
        // {
        //     var invoiceResult = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        //     return invoiceResult.Match(
        //         invoice => Ok(ApiResponse<GetInvoiceDTO>.Ok(invoice)),
        //         Problem
        //     );
        // }

        [HttpPut("edit-invoice")]
        public async Task<IActionResult> EditInvoice(EditInvoiceDTO dto)
        {
            var command = _mapper.Map<EditInvoiceCommand>(dto);
            ErrorOr<InvoiceResult> invoiceResult = await _mediatr.Send(command);
            return invoiceResult.Match(
                success => Ok(ApiResponse<InvoiceResult>.Ok(invoiceResult.Value, "Invoice successfully edited!")),
                Problem
            );
        }   
    }
}
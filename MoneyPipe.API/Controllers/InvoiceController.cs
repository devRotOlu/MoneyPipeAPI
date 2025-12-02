// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using MoneyPipe.API.Common.Http;
// using MoneyPipe.API.DTOs;
// using MoneyPipe.Application.Interfaces.IServices;


// namespace MoneyPipe.API.Controllers
// {
//     [Route("api/[controller]")]
//     [Authorize]
//     public class InvoiceController(IInvoiceService invoiceService): APIController
//     {
//         private readonly IInvoiceService _invoiceService = invoiceService;

//         [HttpPost("create-invoice")]
//         public async Task<IActionResult> CreateInvoice(CreateInvoiceDTO dto)
//         {
//             var invoiceResult = await _invoiceService.CreateInvoiceAsync(dto);

//             return invoiceResult.Match(
//                 invoice => Ok(ApiResponse<GetInvoiceDTO>.Ok(invoice,"Invoice Created Successfully!")),
//                 Problem
//             );
//         }

//         [HttpGet("get-invoices")]
//         public async Task<IActionResult> GetInvoices()
//         {
//             var invoices = await _invoiceService.GetInvoicesAsync(User);

//             return Ok(ApiResponse<IEnumerable<GetInvoiceDTO>>.Ok(invoices));
//         }

//         [HttpGet("get-invoice")]
//         public async Task<IActionResult> GetInvoice([FromQuery] string invoiceId)
//         {
//             var invoiceResult = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
//             return invoiceResult.Match(
//                 invoice => Ok(ApiResponse<GetInvoiceDTO>.Ok(invoice)),
//                 Problem
//             );
//         }

//         [HttpPut("edit-invoice")]
//         public async Task<IActionResult> EditInvoice(EditInvoiceDTO dto)
//         {
//             var invoiceResult = await _invoiceService.EditInvoiceItemAsync(dto);
//             return invoiceResult.Match(
//                 success => Ok(ApiResponse<object>.Ok(null, "Invoice successfully edited!")),
//                 Problem
//             );
//         }   
//     }
// }
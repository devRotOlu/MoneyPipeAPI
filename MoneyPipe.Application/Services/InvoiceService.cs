// using System.Security.Claims;
// using AutoMapper;
// using ErrorOr;
// using MoneyPipe.Application.DTOs;
// using MoneyPipe.Application.Interfaces;
// using MoneyPipe.Application.Interfaces.IServices;
// using MoneyPipe.Domain.Common.Errors;
// using MoneyPipe.Domain.Entities;
// using MoneyPipe.Domain.Models;

// namespace MoneyPipe.Application.Services
// {
//     public class InvoiceService(IUnitOfWork unitOfWork,IMapper mapper) : IInvoiceService
//     {
//         private readonly IMapper _mapper = mapper;
//         private readonly IUnitOfWork _unitofWork = unitOfWork; 

//         public async Task<ErrorOr<GetInvoiceDTO>> CreateInvoiceAsync(CreateInvoiceDTO request)
//         {
//             var invoiceRequest = _mapper.Map<InvoiceRequest>(request);

//             ErrorOr<Invoice> invoiceResult = Invoice.Create(invoiceRequest);

//             if (invoiceResult.IsError) return invoiceResult.Errors;

//             var invoice = invoiceResult.Value;

//             var user = await _unitofWork.Users.GetByEmailAsync(request.CustomerEmail);

//             if (user is not null) invoice.SetUserId(user.Id);

//             int serialNumber = await _unitofWork.Invoices.GetNextInvoiceNumberAsync();
//             invoice.SetInvoiceNumber(serialNumber);

//             await _unitofWork.Invoices.SaveInvoiceAsync(invoice);
//             _unitofWork.Commit();

//             var _invoice = _mapper.Map<GetInvoiceDTO>(invoice);

//             return _invoice;
//         }

//         public async Task<IEnumerable<GetInvoiceDTO>> GetInvoicesAsync(ClaimsPrincipal user)
//         {
//             var _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//             var userId = Guid.Parse(_userId!);

//             var invoices = await _unitofWork.Invoices.GetInvoicesByUserAsync(userId);

//             var _invoices = _mapper.Map<IEnumerable<GetInvoiceDTO>>(invoices);

//             return _invoices;
//         }

//         public async Task<ErrorOr<GetInvoiceDTO?>> GetInvoiceByIdAsync(string invoiceId)
//         {
//             var isParsed = Guid.TryParse(invoiceId, out var _invoiceId);
//             if (!isParsed) return Errors.Invoice.NotFound;
//             var invoice = await _unitofWork.Invoices.GetInvoiceByIdAsync(_invoiceId);
//             var _invoice = _mapper.Map<GetInvoiceDTO>(invoice);
//             return _invoice;
//         }

//         public async Task<ErrorOr<Success>> MarkAsPaidAsync(Guid invoiceId)
//         {
//             var invoice = await _unitofWork.Invoices.GetInvoiceByIdAsync(invoiceId);
//             if (invoice == null) return Errors.Invoice.NotFound;
//             invoice.MarkAsPaid();
//             await _unitofWork.Invoices.UpdateInvoiceAsync(invoice);
//             _unitofWork.Commit();
//             return Result.Success;
//         }

//         public async Task<ErrorOr<Success>> EditInvoiceItemAsync(EditInvoiceDTO dto)
//         {
//             var isParsed = Guid.TryParse(dto.Id, out var invoiceId);
//             Invoice? oldInvoice = null;
//             if (isParsed) oldInvoice = await _unitofWork.Invoices.GetByIdAsync(invoiceId);
//             if (!isParsed || oldInvoice is null) return Errors.Invoice.NotFound;
//             var editedInvoice = _mapper.Map<EditInvoiceRequest>(dto);
//             editedInvoice.Id = invoiceId;
//             var existingItemIds = oldInvoice.InvoiceItems.Select(item => item.Id).ToHashSet();
//             foreach (var item in editedInvoice.InvoiceItems)
//             {
//                 if (!string.IsNullOrEmpty(item.InvoiceItemId))
//                 {
//                     var idIsParsed = Guid.TryParse(item.InvoiceItemId, out var itemId);
//                     if (!idIsParsed || !existingItemIds.Contains(itemId))
//                     {
//                         return Error.NotFound(Errors.InvoiceItem.NotFound.Code,"One or more invoice items could not be found or are invalid.");
//                     }
//                     item.Id = itemId;
//                 }
//             }
//             var invoiceResult = Invoice.Edit(editedInvoice);
//             if (invoiceResult.IsError) return invoiceResult.Errors;
//             await _unitofWork.Invoices.EditInvoiceAsync(invoiceResult.Value);
//             return Result.Success;
//         }
//     }

// }
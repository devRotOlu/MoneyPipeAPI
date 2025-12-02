// using System.Security.Claims;
// using ErrorOr;
// using MoneyPipe.Application.DTOs;
// using MoneyPipe.Domain.Entities;

// namespace MoneyPipe.Application.Interfaces.IServices
// {
//         public interface IInvoiceService
//     {
//         Task<ErrorOr<GetInvoiceDTO>> CreateInvoiceAsync(CreateInvoiceDTO request);
//         Task<IEnumerable<GetInvoiceDTO>> GetInvoicesAsync(ClaimsPrincipal user);
//         Task<ErrorOr<GetInvoiceDTO?>> GetInvoiceByIdAsync(string invoiceId);
//         Task<ErrorOr<Success>> MarkAsPaidAsync(Guid invoiceId);
//         Task<ErrorOr<Success>> EditInvoiceItemAsync(EditInvoiceDTO dto);
//     }

// }
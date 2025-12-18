using System.Security.Claims;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Invoicing.Common;
using MoneyPipe.Domain.InvoiceAggregate;
using MoneyPipe.Domain.InvoiceAggregate.Models;


namespace MoneyPipe.Application.Services.Invoicing.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
    IInvoiceReadRepository invoiceQuery,IHttpContextAccessor httpContextAccessor) : 
    IRequestHandler<CreateInvoiceCommand, ErrorOr<InvoiceResult>>
    {

        private readonly IUnitOfWork _unitofWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ErrorOr<InvoiceResult>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var invoiceData = _mapper.Map<InvoiceData>(request);
            ErrorOr<Invoice> invoicingResult = Invoice.Create(invoiceData);
            if (invoicingResult.IsError) return invoicingResult.Errors;
            
            var invoice = invoicingResult.Value;

            var invoiceNumber = await _invoiceQuery.GetNextInvoiceNumberAsync();

            invoice.SetInvoiceNumber(invoiceNumber);
            invoice.SetUserId(Guid.Parse(userId));

            await _unitofWork.Invoices.InsertAsync(invoice);
            await _unitofWork.Commit();

            var invoiceResult = _mapper.Map<InvoiceResult>(invoice);

            return invoiceResult;
        }
    }
}
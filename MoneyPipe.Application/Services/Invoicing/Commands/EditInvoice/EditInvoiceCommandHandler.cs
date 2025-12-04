using System.Security.Claims;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Invoicing.Commands.Common;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.InvoiceAggregate;
using MoneyPipe.Domain.InvoiceAggregate.Models;

namespace MoneyPipe.Application.Services.Invoicing.Commands.EditInvoice
{
    public class EditInvoiceCommandHandler(IInvoiceReadRepository invoiceQuery, IMapper mapper,
    IUnitOfWork unitofWork, IHttpContextAccessor httpContextAccessor) : 
    IRequestHandler<EditInvoiceCommand, ErrorOr<InvoiceResult>>
    {
        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery; 
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitofWork = unitofWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ErrorOr<InvoiceResult>> Handle(EditInvoiceCommand request, CancellationToken cancellationToken)
        {
            var isInvoiceId = Guid.TryParse(request.InvoiceId,out var invoiceId);
            Invoice? existingInvoice = null;
            if (isInvoiceId) existingInvoice = await _invoiceQuery.GetByIdAsync(invoiceId);

            if (!isInvoiceId || existingInvoice is null) return Errors.Invoice.NotFound;

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var invoiceData = _mapper.Map<EditInvoiceData>(request);

            var editResult = Invoice.Edit(invoiceData);

            if (editResult.IsError) return editResult.Errors;

            var editedInvoice = editResult.Value;
            editedInvoice.SetUserId(Guid.Parse(userId));

            await _unitofWork.Invoices.UpdateAsync(editedInvoice);
            _unitofWork.Commit();

            var invoiceResult = _mapper.Map<InvoiceResult>(editedInvoice);

            return invoiceResult ;
        }
    }
}
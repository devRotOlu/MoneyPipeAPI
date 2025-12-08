using AutoMapper;
using MediatR;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Invoicing.Common;

namespace MoneyPipe.Application.Services.Invoicing.Queries.GetInvoice
{
    public class GetInvoicesQueryHandler(IInvoiceReadRepository invoiceQuery,IMapper mapper) 
    : IRequestHandler<GetInvoiceQuery, InvoiceResult?>
    {

        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery;
        private readonly IMapper _mapper = mapper;
        public async Task<InvoiceResult?> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceQuery.GetByIdAsync(request.InvoiceId);
            var invoiceResult = _mapper.Map<InvoiceResult>(invoice);
            return invoiceResult;
        }
    }
}
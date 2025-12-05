using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Services.Invoicing.Common;

namespace MoneyPipe.Application.Services.Invoicing.Queries.GetInvoices
{
    public class GetInvoicesQueryHandler(IInvoiceReadRepository invoiceQuery, 
    HttpContextAccessor httpContextAccessor,IMapper mapper) 
    : IRequestHandler<GetInvoicesQuery, GetInvoicesResult>
    {
        private readonly IInvoiceReadRepository _invoiceQuery = invoiceQuery;
        private readonly HttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        private const int _defaultPageSize = 20;
        private const int _maximumPageSize = 50; 
        public async Task<GetInvoicesResult> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pageSize = request.PageSize is null? _defaultPageSize :
             request.PageSize <= _maximumPageSize? request.PageSize : _maximumPageSize;
            
            var invoices = await _invoiceQuery.GetInvoicesAsync(Guid.Parse(userId),pageSize??0,
            request.LastTimestamp);

            var invoiceResult = _mapper.Map<IEnumerable<InvoiceResult>>(invoices);

            if (!invoiceResult.Any()) return new GetInvoicesResult(invoiceResult,null);

            var invoice = invoices.ToList().Last();

            var lastTimestamp = invoice.CreatedAt;

            return new GetInvoicesResult(invoiceResult,lastTimestamp);
        }
    }
}
using System.Text.Json;
using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure
{
    public class BackgroundJobQueue(IUnitOfWork unitofWork) : IBackgroundJobQueue
    {
        private readonly IUnitOfWork _unitofWork = unitofWork;

        public async Task EnqueueSendInvoiceAsync(InvoiceId invoiceId)
        {
            var invoiceJobPayload = new InvoiceJobPayload(invoiceId.Value.ToString());
            var json = invoiceJobPayload.ToString();
            var payload = JsonDocument.Parse(json);
            var backgroundJob = BackgroundJob.Create(JobTypes.SendInvoice);
            backgroundJob.AddPayload(payload);
            await _unitofWork.BackgroundJobs.CreateBackgroundJobAsync(backgroundJob);
        }
    }
}
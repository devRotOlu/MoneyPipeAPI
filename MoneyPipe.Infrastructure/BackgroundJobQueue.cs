using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure
{
    public class BackgroundJobQueue(IUnitOfWork unitofWork) : IBackgroundJobQueue
    {
        private readonly IUnitOfWork _unitofWork = unitofWork;

        public async Task EnqueueSendInvoiceAsync(InvoiceId invoiceId)
        {
            var backgroundJob = BackgroundJob.Create(JobTypes.SendInvoice,invoiceId);
            await _unitofWork.BackgroundJobs.CreateBackgroundJobAsync(backgroundJob);
        }
    }
}
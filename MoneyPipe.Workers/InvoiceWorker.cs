using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.BackgroundJobAggregate.Enums;
using MoneyPipe.Domain.EmailJobAggregate;
using MoneyPipe.Domain.InvoiceAggregate;

public class InvoiceWorker : BackgroundService
{
    private readonly ILogger<InvoiceWorker> _logger;
    private readonly IInvoicePdfGenerator _pdfGenerator;
    private readonly ICloudinaryService _cloudinary;
    private readonly IBackgroundJobReadRepository _jobReadRepository;
    private readonly IServiceProvider _serviceProvider;
    

    public InvoiceWorker(ILogger<InvoiceWorker> logger,IInvoicePdfGenerator pdfGenerator,
    ICloudinaryService cloudinary,
    IBackgroundJobReadRepository jobReadRepository,IServiceProvider serviceProvider)
    {
        _logger = logger;
        _pdfGenerator = pdfGenerator;
        _cloudinary = cloudinary;
        _jobReadRepository = jobReadRepository;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Invoice Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var invoiceJobs = await _jobReadRepository.GetUnCompletedBackgroundJobsAsync(JobTypes.SendInvoice);
            if (!invoiceJobs.Any())
            {
                await Task.Delay(3000, stoppingToken);
                continue;
            }
            await ProcessJobAsync(invoiceJobs);
        }
    }

    private async Task ProcessJobAsync(IEnumerable<BackgroundJob> jobs)
    {
        foreach (var job in jobs)
        {           
            try
            {
                var invoice = await FetchInvoice(job);

                if (invoice!.PDFLink is null)
                {

                    var pdfBytes = _pdfGenerator.GeneratePdf(invoice!);
                    var pdfUrl = await _cloudinary.UploadPdfAsync(pdfBytes, invoice!.InvoiceNumber);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        invoice.SetPDFLink(pdfUrl);

                        await uow.Invoices.UpdateAsync(invoice);
                        uow.Commit();
                    }

                    await MarkJobCompleted(job, invoice);
                }
                else
                {
                    await MarkJobCompleted(job, invoice!);
                }
            }
            catch
            {
                try
                {
                    await MarkJobFailed(job);
                }
                catch { }
            }
        }
    }

    private async Task MarkJobFailed(BackgroundJob job)
    {
        using var scope = _serviceProvider.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        job.UpdateStatus(StatusTypes.Failed);
        await uow.BackgroundJobs.UpdateBackgroundJobAsync(job);
        uow.Commit();
    }

    private async Task MarkJobCompleted(BackgroundJob job, Invoice invoice)
    {
        using var scope = _serviceProvider.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var result = EmailJob.Create(
            invoice.CustomerEmail,
            $"Invoice {invoice.InvoiceNumber}",
            $"Download invoice here: {invoice.PDFLink}"
        );

        if (!result.IsError)
            await uow.EmailJobs.CreateEmailJobAsync(result.Value);

        job.UpdateStatus(StatusTypes.Completed);
        await uow.BackgroundJobs.UpdateBackgroundJobAsync(job);

        uow.Commit();
    }

    private async Task<Invoice?> FetchInvoice(BackgroundJob job)
    {
        Invoice? invoice;
        using (var scope = _serviceProvider.CreateScope())
        {
            var readRepo = scope.ServiceProvider.GetRequiredService<IInvoiceReadRepository>();
            invoice = await readRepo.GetByIdAsync(job.InvoiceId.Value);

            if (invoice is null)
                throw new Exception("Invoice not found");
        }

        return invoice;
    }
}

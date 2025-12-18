using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.EmailJobAggregate;
using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Workers
{
    public class InvoiceWorker(ILogger<InvoiceWorker> logger, IInvoicePdfGenerator pdfGenerator,
    ICloudinaryService cloudinary, IServiceProvider serviceProvider,IEmailTemplateService emailTemplateService) 
    : BackgroundService
    {
        private readonly ILogger<InvoiceWorker> _logger = logger;
        private readonly IInvoicePdfGenerator _pdfGenerator = pdfGenerator;
        private readonly ICloudinaryService _cloudinary = cloudinary;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Invoice Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                IEnumerable<BackgroundJob> invoiceJobs = [];

                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobRepo = scope.ServiceProvider.GetRequiredService<IBackgroundJobReadRepository>();
                    invoiceJobs = await jobRepo.GetUnCompletedBackgroundJobsAsync(JobTypes.SendInvoice);
                }

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
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    try
                    {
                        await MarkJobFailed(job);
                    }
                    catch(Exception ey)
                    {
                        _logger.LogInformation("Error occurred!");
                         Console.WriteLine(ey.Message);
                    }
                }
            }
        }

        private async Task MarkJobFailed(BackgroundJob job)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            job.MarkCompleted(false);
            await uow.BackgroundJobs.UpdateBackgroundJobAsync(job);
            uow.Commit();
        }

        private async Task MarkJobCompleted(BackgroundJob job, Invoice invoice)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var emailOption = _emailTemplateService.BuildInvoiceDeliveryEmail(invoice.CustomerName,
            invoice.CustomerEmail,invoice.InvoiceNumber,invoice.PDFLink!);

            var result = EmailJob.Create(emailOption.ToEmail,emailOption.Message,
            emailOption.Subject);

            if (!result.IsError)
            {
                var emailjob = result.Value;
                emailjob.AddHTMLContent(emailOption.HtmlContent!);
                await uow.EmailJobs.CreateEmailJobAsync(emailjob);
            }

            job.MarkCompleted(true);
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
}

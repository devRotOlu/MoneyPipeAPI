using MoneyPipe.Application.Common;
using MoneyPipe.Application.Interfaces;
using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Application.Models;
using MoneyPipe.Application.Services;
using MoneyPipe.Domain.BackgroundJobAggregate;
using MoneyPipe.Domain.EmailJobAggregate;
using MoneyPipe.Domain.InvoiceAggregate;
using MoneyPipe.Domain.InvoiceAggregate.Enums;
using MoneyPipe.Domain.InvoiceAggregate.ValueObjects;
using MoneyPipe.Infrastructure.Persistence.Repositories.Reads;

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

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _logger.LogInformation("Invoice Worker started.");

            while (!ct.IsCancellationRequested)
            {
                IEnumerable<BackgroundJob> invoiceJobs = [];

                using (var scope = _serviceProvider.CreateScope())
                {
                    var jobRepo = scope.ServiceProvider.GetRequiredService<IBackgroundJobReadRepository>();
                    invoiceJobs = await jobRepo.GetUnCompletedBackgroundJobsAsync(JobTypes.SendInvoice);
                }

                if (!invoiceJobs.Any())
                {
                    await Task.Delay(3000, ct);
                    continue;
                }
                await ProcessJobAsync(invoiceJobs,ct);
            }
        }

        private async Task ProcessJobAsync(IEnumerable<BackgroundJob> jobs,CancellationToken ct)
        {
            foreach (var job in jobs)
            {           
                try
                {
                    var invoicePayload = InvoiceJobPayload.Deserialize(job.Payload!);
                    var invoiceId = invoicePayload!.InvoiceId;
                    var invoice = await FetchInvoice(invoiceId!);
                

                    if (invoice!.PDFLink is null)
                    {
                        var paymentMethod = invoicePayload.PaymentMethod;
                        var walletId = invoicePayload.WalletId;
                        VirtualAccount? activeAccount = null;

                        if (paymentMethod == PaymentMethod.Card && invoice!.PaymentUrl is null)
                            await AddPaymentLink(invoice!,ct);
                        else
                            activeAccount = await GetAccountDetaiils(invoice!, walletId, activeAccount);

                        var pdfBytes = _pdfGenerator.GeneratePdf(invoice!,invoice.PaymentUrl,activeAccount);
                        var pdfUrl = await _cloudinary.UploadPdfAsync(pdfBytes, invoice!.InvoiceNumber);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                            invoice.SetPDFLink(pdfUrl);

                            await uow.Invoices.UpdateAsync(invoice);
                            await uow.Commit();
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

        private async Task<VirtualAccount?> GetAccountDetaiils(Invoice invoice, Domain.WalletAggregate.ValueObjects.WalletId walletId, VirtualAccount? activeAccount)
        {
            using var _scope = _serviceProvider.CreateScope();
            var walletRepo = _serviceProvider.GetRequiredService<WalletReadRepository>();
            var wallet = await walletRepo.GetWallet(walletId);
            var virtualAccount = wallet!.VirtualAccounts.First(account => account.IsActive);
            activeAccount = new VirtualAccount(virtualAccount.BankName,
            virtualAccount.ProviderAccountId, virtualAccount.AccountName);
            invoice!.AddPaymentDetails(PaymentMethod.BankTransfer, virtualAccount.ProviderName, null);
            return activeAccount;
        }

        private async Task AddPaymentLink(Invoice invoice,CancellationToken ct)
        {
            using var _scope = _serviceProvider.CreateScope();
            var accountProcessorResolver = _scope.ServiceProvider.GetRequiredService<PaymentCreationResolver>();
            var processor = accountProcessorResolver.Resolve();
            invoice!.AddPaymentReference();
            var response = await processor.ProcessPaymentCreation(invoice.PaymentReference!, 
            invoice.TotalAmount ?? 0, invoice.CustomerEmail, invoice.Currency!,ct);
            invoice.AddPaymentDetails(PaymentMethod.Card, response.PaymentProvider, response.PaymentLink);
        }

        private async Task MarkJobFailed(BackgroundJob job)
        {
            using var scope = _serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            job.MarkCompleted(false);
            await uow.BackgroundJobs.UpdateBackgroundJobAsync(job);
            await uow.Commit();
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

            await uow.Commit();
        }

        private async Task<Invoice?> FetchInvoice(InvoiceId invoiceId)
        {
            Invoice? invoice;
            using (var scope = _serviceProvider.CreateScope())
            {
                var readRepo = scope.ServiceProvider.GetRequiredService<IInvoiceReadRepository>();
                invoice = await readRepo.GetByIdAsync(invoiceId.Value) ?? throw new Exception("Invoice not found");
            }
            return invoice;
        }
    }
}

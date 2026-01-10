using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Application.Models;
using MoneyPipe.Domain.InvoiceAggregate;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MoneyPipe.Application.Services
{
    public class InvoicePdfGenerator : IInvoicePdfGenerator
    {
        public byte[] GeneratePdf(Invoice invoice,string? paymentUrl,VirtualAccount? virtualAccount)
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // HEADER
                    page.Header()
                        .Text($"Invoice: {invoice.InvoiceNumber}")
                        .SemiBold()
                        .FontSize(20);

                    // CONTENT
                    page.Content()
                        .Column(col =>
                        {
                            col.Spacing(10);

                            col.Item().Text($"Issue Date: {invoice.IssueDate:yyyy-MM-dd}");
                            col.Item().Text($"Due Date: {invoice.DueDate:yyyy-MM-dd}");
                            col.Item().Text($"Customer: {invoice.CustomerName} ({invoice.CustomerEmail})");

                            // INVOICE ITEMS
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Description").SemiBold();
                                    header.Cell().Text("Qty").SemiBold();
                                    header.Cell().Text("Unit Price").SemiBold();
                                    header.Cell().Text("Total").SemiBold();
                                });

                                foreach (var item in invoice.InvoiceItems)
                                {
                                    table.Cell().Text(item.Description);
                                    table.Cell().Text(item.Quantity?.ToString() ?? "-");
                                    table.Cell().Text(item.UnitPrice?.ToString("C") ?? "-");
                                    table.Cell().Text(item.TotalPrice?.ToString("C") ?? "-");
                                }
                            });

                            col.Item().Text($"Subtotal: {invoice.SubTotal:C}");
                            col.Item().Text($"Tax: {invoice.TaxAmount:C}");
                            col.Item().Text($"Total: {invoice.TotalAmount:C}").Bold();

                            // PAYMENT SECTION (OPTIONAL)
                            if (!string.IsNullOrWhiteSpace(paymentUrl) ||
                                virtualAccount != null)
                            {
                                col.Item().PaddingTop(20).LineHorizontal(1);

                                col.Item().Text("Payment Information")
                                    .SemiBold()
                                    .FontSize(14);

                                // CARD PAYMENT LINK
                                if (!string.IsNullOrWhiteSpace(paymentUrl))
                                {
                                    col.Item().Text("Pay by Card:")
                                        .SemiBold();

                                    col.Item().Hyperlink(paymentUrl)
                                        .Text(paymentUrl)
                                        .FontColor(Colors.Blue.Medium);
                                }

                                // BANK TRANSFER DETAILS
                                if (virtualAccount != null)
                                {
                                    col.Item().PaddingTop(10).Text("Pay by Bank Transfer:")
                                        .SemiBold();

                                    col.Item().Text($"Bank: {virtualAccount.BankName}");
                                    col.Item().Text($"Account Number: {virtualAccount.AccountNumber}");
                                    col.Item().Text($"Account Name: {virtualAccount.AccountName}");
                                }
                            }
                        });

                    // FOOTER
                    page.Footer()
                        .AlignCenter()
                        .Text("Thank you for your business!");
                });
            }).GeneratePdf();

            return pdfBytes;
        }

    }
}
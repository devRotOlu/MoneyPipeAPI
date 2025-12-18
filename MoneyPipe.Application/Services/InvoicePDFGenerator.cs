using MoneyPipe.Application.Interfaces.IServices;
using MoneyPipe.Domain.InvoiceAggregate;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MoneyPipe.Application.Services
{
    public class InvoicePdfGenerator : IInvoicePdfGenerator
    {
        public byte[] GeneratePdf(Invoice invoice)
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Invoice: {invoice.InvoiceNumber}")
                        .SemiBold().FontSize(20);

                    page.Content()
                        .Column(col =>
                        {
                            col.Item().Text($"Issue Date: {invoice.IssueDate:yyyy-MM-dd}");
                            col.Item().Text($"Due Date: {invoice.DueDate:yyyy-MM-dd}");
                            col.Item().Text($"Customer: {invoice.CustomerName} ({invoice.CustomerEmail})");

                            col.Item().Table(table =>
                            {
                                // Columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4); // Description
                                    columns.RelativeColumn(1); // Quantity
                                    columns.RelativeColumn(2); // Unit Price
                                    columns.RelativeColumn(2); // Total
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Text("Description").SemiBold();
                                    header.Cell().Text("Qty").SemiBold();
                                    header.Cell().Text("Unit Price").SemiBold();
                                    header.Cell().Text("Total").SemiBold();
                                });

                                // Items
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
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text("Thank you for your business!");
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
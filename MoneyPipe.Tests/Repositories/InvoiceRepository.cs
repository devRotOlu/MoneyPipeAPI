// namespace MoneyPipe.Tests.Repositories
// {
//     public class InvoiceRepositoryTests
// {
//     private IDatabaseConnectionFactory _factory;
//     private IInvoiceRepository _repo;

//     public InvoiceRepositoryTests()
//     {
//         // Use in-memory SQLite for testing
//         var connection = new SqliteConnection("Data Source=:memory:");
//         connection.Open();
//         // Create tables (simplified SQL)
//         connection.Execute(@"
//             CREATE TABLE Invoices (
//                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                 InvoiceNumber TEXT UNIQUE, UserId INTEGER NULL,
//                 CustomerEmail TEXT, Subtotal DECIMAL, TaxAmount DECIMAL,
//                 TotalAmount DECIMAL, CreatedAt TEXT, PaidAt TEXT
//             );
//             CREATE TABLE InvoiceItems (
//                 Id INTEGER PRIMARY KEY AUTOINCREMENT,
//                 InvoiceId INTEGER, Quantity INTEGER,
//                 UnitPrice DECIMAL, TotalPrice DECIMAL
//             );");
//         _factory = new ConnectionFactoryMock(connection);
//         _repo = new InvoiceRepository(_factory);
//     }

//     [Fact]
//     public async Task SaveInvoice_WithItems_InsertsBothTables()
//     {
//         // Arrange: create invoice with items
//         var invoice = new Invoice {
//             CustomerEmail = "bob@example.com",
//             UserId = null, CreatedAt = DateTime.UtcNow,
//             InvoiceNumber = "INV-000001",
//             Subtotal = 200m, TaxAmount = 20m, TotalAmount = 220m
//         };
//         invoice.Items.Add(new InvoiceItem { Quantity = 2, UnitPrice = 100m, TotalPrice = 200m });

//         // Act: save
//         await _repo.SaveInvoiceAsync(invoice);

//         // Assert: retrieve and compare
//         var saved = await _repo.GetInvoiceByIdAsync(invoice.Id);
//         Assert.NotNull(saved);
//         Assert.Equal(200m, saved.Subtotal);
//         Assert.Single(saved.Items);
//         Assert.Equal(220m, saved.TotalAmount);
//     }

//     // Additional tests: GetInvoicesByUserAsync, pagination, etc.
// }

// }
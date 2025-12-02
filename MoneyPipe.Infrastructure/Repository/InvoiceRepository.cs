using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.IRepository;
using MoneyPipe.Domain.Entities;


namespace MoneyPipe.Infrastructure.Repository
{
    public class InvoiceRepository(IDbConnection dbConnection, IDbTransaction transaction) : GenericRepository<Invoice>(dbConnection, transaction, "Invoices"),IInvoiceRepository
    {
        private readonly string _invoiceItemsTable = "InvoiceItems";
        public async Task<int> GetNextInvoiceNumberAsync()
        {
            var maxId = await _dbConnection.QuerySingleAsync<int>(
                $"SELECT COALESCE(MAX(Id), 0) FROM {_tableName}");
            return maxId + 1;
        }

        public async Task SaveInvoiceAsync(Invoice invoice)
        {
            // Insert invoice, get Id
            var insertSql = $@"
                INSERT INTO {_tableName} (InvoiceNumber, UserId, CustomerEmail, Subtotal, TaxAmount, TotalAmount, CreatedAt)
                VALUES (@InvoiceNumber, @UserId, @CustomerEmail, @Subtotal, @TaxAmount, @TotalAmount, @CreatedAt);
                SELECT last_insert_rowid();"; // SQLite; use RETURNING Id for PostgreSQL
            var id = await _dbConnection.ExecuteScalarAsync<Guid>(
                insertSql, invoice, _transaction);

            // Insert items
            foreach (var item in invoice.InvoiceItems)
            {
                var itemSql = $@"
                    INSERT INTO {_invoiceItemsTable} (InvoiceId, Quantity, UnitPrice, TotalPrice)
                    VALUES ({id}, @Quantity, @UnitPrice, @TotalPrice);";
                await _dbConnection.ExecuteAsync(itemSql, item, _transaction);
            }
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(Guid id)
        {
            var sql = $@"
                SELECT 
                    i.*,
                    ii.*
                FROM {_tableName} i
                LEFT JOIN {_invoiceItemsTable} ii ON i.Id = ii.InvoiceId
                WHERE i.Id = @Id;
            ";

            var invoiceLookup = new Dictionary<Guid, Invoice>();

            await _dbConnection.QueryAsync<Invoice, InvoiceItem, Invoice>(
                sql,
                (invoice, item) =>
                {
                    if (!invoiceLookup.TryGetValue(invoice.Id, out var invoiceEntry))
                    {
                        invoiceEntry = invoice;
                        //invoiceEntry.SetInvoiceItems([]);
                        invoiceLookup.Add(invoiceEntry.Id, invoiceEntry);
                    }

                    if (item != null)
                        invoiceEntry.AddInvoiceItem(item);

                    return invoiceEntry;
                },
                new { Id = id },
                splitOn: "Id" // tells Dapper that InvoiceItem starts at its Id column
            );

            return invoiceLookup.Values.FirstOrDefault();
        }


        public async Task<IEnumerable<Invoice>> GetInvoicesByUserAsync(Guid userId)
        {
            var invoices = await _dbConnection.QueryAsync<Invoice>(
                $@"SELECT * FROM {_tableName} WHERE UserId = @UserId",
                new { UserId = userId });
            // Optionally load items or skip for listing view
            return invoices;
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            await _dbConnection.ExecuteAsync(@"
                UPDATE Invoices SET PaidAt = @PaidAt WHERE Id = @Id",
                new { invoice.PaidAt, invoice.Id });
        }

        public async Task EditInvoiceAsync(Invoice invoice)
        {
            // Update invoice fields
            var updateInvoiceSql = $@"
                UPDATE {_tableName}
                SET InvoiceNumber=@InvoiceNumber, UserId=@UserId, CustomerEmail=@CustomerEmail,
                    Subtotal=@Subtotal, TaxAmount=@TaxAmount, TotalAmount=@TotalAmount, UpdatedAt=@UpdatedAt
                WHERE Id=@Id";
            await _dbConnection.ExecuteAsync(updateInvoiceSql, invoice, _transaction);

            // Load existing items from DB
            var existingItems = await _dbConnection.QueryAsync<InvoiceItem>(
                $@"SELECT * FROM {_invoiceItemsTable} WHERE InvoiceId=@InvoiceId",
                new { InvoiceId = invoice.Id });

            var existingItemsDict = existingItems.ToDictionary(i => i.Id);

            foreach (var item in invoice.InvoiceItems)
            {
                if (item.Id == Guid.Empty || !existingItemsDict.ContainsKey(item.Id))
                {
                    // New item -> INSERT
                    var insertSql = $@"
                        INSERT INTO {_invoiceItemsTable} (InvoiceId, Quantity, UnitPrice, TotalPrice, Description)
                        VALUES (@InvoiceId, @Quantity, @UnitPrice, @TotalPrice, @Description)";
                    await _dbConnection.ExecuteAsync(insertSql, new { item.InvoiceId, item.Quantity, item.UnitPrice, item.TotalPrice, item.Description }, _transaction);
                }
                else
                {
                    // Existing item -> UPDATE
                    var updateSql = $@"
                        UPDATE {_invoiceItemsTable}
                        SET Quantity=@Quantity, UnitPrice=@UnitPrice, TotalPrice=@TotalPrice, Description=@Description
                        WHERE Id=@Id";
                    await _dbConnection.ExecuteAsync(updateSql, item, _transaction);

                    // Remove from dictionary to track deletions
                    existingItemsDict.Remove(item.Id);
                }
            }

            // Delete items that were removed in DTO
            foreach (var itemToDelete in existingItemsDict.Values)
            {
                var deleteSql = $"DELETE FROM {_invoiceItemsTable} WHERE Id=@Id";
                await _dbConnection.ExecuteAsync(deleteSql, new { itemToDelete.Id }, _transaction);
            }
        }
    }
}
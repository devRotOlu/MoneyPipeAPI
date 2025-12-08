using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.InvoiceAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Writes
{
    public class InvoiceWriteRepository(IDbConnection dbConnection, IDbTransaction transaction) : IInvoiceWriteRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IDbTransaction _transaction = transaction;
        private readonly string _invoiceTable = "Invoices";
        private readonly string _invoiceItemTable = "InvoiceItems";
        public async Task DeleteAsync(Invoice invoice)
        {
            await _dbConnection.ExecuteAsync(@$"DELETE FROM {_invoiceTable} WHERE id = @Id", invoice,_transaction);
        }

        public async Task InsertAsync(Invoice invoice)
        {
            var invoiceSql = @$"
            INSERT INTO {_invoiceTable}
            (id, userid, invoicenumber, subtotal, taxamount, totalamount, currency, status,
             duedate, customername, customeremail, customeraddress, notes,
              paymenturl, createdat)VALUES
            (@Id, @UserId, @InvoiceNumber, @SubTotal, @TaxAmount, @TotalAmount, @Currency, @Status,
             @DueDate, @CustomerName, @CustomerEmail, @CustomerAddress, @Notes, @PaymentUrl,@CreatedAt);";
            await _dbConnection.ExecuteAsync(invoiceSql,invoice,_transaction);

            foreach (var item in invoice.InvoiceItems)
            {
                var invoiceItemSql = @$"
                INSERT INTO {_invoiceItemTable} (id, invoiceid, description, quantity, unitprice, totalprice)
                VALUES (@Id, @InvoiceId, @Description, @Quantity, @UnitPrice, @TotalPrice)";
                await _dbConnection.ExecuteAsync(invoiceItemSql,item,_transaction);
            } 
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            var invoiceSql = @$" 
                    UPDATE {_invoiceTable} SET
                    subtotal = @SubTotal,
                    taxamount = @TaxAmount,
                    totalamount = @TotalAmount,
                    currency = @Currency,
                    status = @Status,
                    duedate = @DueDate,
                    issuedate = @IssueDate,
                    paidat = @PaidAt,
                    customername = @CustomerName,
                    customeremail = @CustomerEmail,
                    customeraddress = @CustomerAddress,
                    notes = @Notes,
                    paymenturl = @PaymentUrl,
                    updatedat = @UpdatedAt
                WHERE id = @Id;";
            await _dbConnection.ExecuteAsync(invoiceSql,invoice,_transaction);

            // fetch existing item ids
            var dbItemIds = (await _dbConnection.QueryAsync<Guid>(
            @$"SELECT id FROM {_invoiceItemTable} WHERE invoiceid = @InvoiceId",
            new { InvoiceId = invoice.Id}, _transaction)).ToHashSet();

            // get ids of items in domain but not in db
            var domainItemIds = invoice.InvoiceItems.Select(item=> item.Id.Value).ToHashSet();

            // delete items that exist in db but not in domain
            var toDelete =  dbItemIds.Except(domainItemIds);

            if (toDelete.Any())
            {
                await _dbConnection.ExecuteAsync(@$"DELETE FROM {_invoiceItemTable} WHERE id = ANY(@Ids)",
                new {Ids = toDelete.ToArray()},_transaction);
            }

            // upsert domain items
            foreach (var item in invoice.InvoiceItems)
            {
                if (dbItemIds.Contains(item.Id.Value))
                {
                    var invoiceItemUpdateSql = @$"UPDATE {_invoiceItemTable}
                    SET description = @Description, quantity = @Quantity, unitprice = @UnitPrice, totalprice = @TotalPrice
                    WHERE id = @Id";
                    await _dbConnection.ExecuteAsync(invoiceItemUpdateSql,item,_transaction);
                }
                else
                {
                    var insertItemInsertSql = @$"INSERT INTO {_invoiceItemTable} 
                    (id, invoiceid, description, quantity, unitprice, totalprice)
                    VALUES (@Id, @InvoiceId, @Description, @Quantity, @UnitPrice, @TotalPrice);";
                    await _dbConnection.ExecuteAsync(insertItemInsertSql,item,_transaction);
                }
            }
        }
    }
}
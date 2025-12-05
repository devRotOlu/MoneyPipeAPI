using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.InvoiceAggregate;
using MoneyPipe.Domain.InvoiceAggregate.Entities;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Reads
{
    public class InvoiceReadRepository(IDbConnection dbConnection) : IInvoiceReadRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly string _invoiceTable = "Invoices";
        private readonly string _invoiceItemTable = "InvoiceItems";

        public async Task<Invoice?> GetByIdAsync(Guid invoiceId)
        {
            var sql = $@"SELECT * FROM {_invoiceTable} WHERE id = @Id 
                SELECT * FROM {_invoiceItemTable} WHERE invoiceid = @Id ORDER BY id
            ";
            using var multi = await _dbConnection.QueryMultipleAsync(sql,new {Id = invoiceId});

            var invoice = await multi.ReadFirstOrDefaultAsync<Invoice>();

            if (invoice is null) return null;

            var invoiceItems = (await multi.ReadAsync<InvoiceItem>()).ToList();

            invoice.AddInvoiceItems(invoiceItems);

            return invoice;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync(Guid userId,int pageSize,DateTime? lastTimestamp)
        {
            var sql = @$"SELECT * FROM {_invoiceTable}
                        WHERE userid = @UserId
                        {(lastTimestamp.HasValue ? "AND createdat > @LastTimestamp" : "")}
                        ORDER BY createdat ASC
                        LIMIT @PageSize";

            return await _dbConnection.QueryAsync<Invoice>(sql, 
            new { LastTimestamp = lastTimestamp,UserId = userId,PageSize = pageSize });
        }

        public async Task<int> GetNextInvoiceNumberAsync()
        {
            var maxId = await _dbConnection.QuerySingleAsync<int>(
                @$"SELECT COALESCE(MAX(Id), 0) FROM {_invoiceTable}");
            return maxId + 1;
        }
    }
}
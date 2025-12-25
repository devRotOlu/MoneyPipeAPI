using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.WalletAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Writes
{
    public class WalletWriteRepository(IDbConnection dbConnection,IDbTransaction transaction)
    :IWalletWriteRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IDbTransaction _transaction = transaction;

        private readonly string _walletTable = "Wallets";
        private readonly string _accountTable = "VirtualAccounts";

        public async Task AddVirtualAccount(Wallet wallet)
        {
            foreach (var item in wallet.VirtualAccounts)
            {
                var accountSql = @$"
                INSERT INTO {_accountTable} (id, walletid, bankname, accountname, providername,
                 provideraccountid,isactive, createdat, updatedat, currency)
                VALUES (@Id, @WalletId, @BankName, @AccountName, @ProviderName, 
                @ProviderAccountId, @IsActive, @CreatedAt, @UpdatedAt, @Currency)";
                await _dbConnection.ExecuteAsync(accountSql,item,_transaction);
            } 
        }

        public async Task CreateWallet(Wallet wallet)
        {
            var sql = @$"INSERT INTO {_walletTable}
            (id,userid, balance, isactive, createdat, updatedat, currency)
            VALUES (@Id, @UserId, @Balance, @IsActive, @CreatedAt, @UpdatedAt, @Currency)";
            await _dbConnection.ExecuteAsync(sql, wallet, _transaction); 
        }
    }
}
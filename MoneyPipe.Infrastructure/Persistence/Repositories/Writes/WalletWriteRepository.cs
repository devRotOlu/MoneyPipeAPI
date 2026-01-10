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
        private readonly string _virtualCardTable = "VirtualCards";

        public async Task AddVirtualAccount(Wallet wallet)
        {
            var maxIndex = wallet.VirtualAccounts.Count - 1;
            var accountSql = @$"
                INSERT INTO {_accountTable} (id, walletid, bankname, accountname, providername,
                 provideraccountid,isactive, createdat, updatedat, currency,isprimaryforinvoice)
                VALUES (@Id, @WalletId, @BankName, @AccountName, @ProviderName, 
                @ProviderAccountId, @IsActive, @CreatedAt, @UpdatedAt, @Currency,
                @IsPrimaryForInvoice)";
            await _dbConnection.ExecuteAsync(accountSql,wallet.VirtualAccounts[maxIndex],_transaction);
        }

        public async Task AddVirtualCard(Wallet wallet,CancellationToken ct)
        { 
            var maxIndex = wallet.VirtualCards.Count - 1;
            var cardSql = $@"INSERT INTO {_virtualCardTable} (id, walletid,
                cardnumber, expirymonth, expiryyear, cvc, status, currency, limit,
                createdat, updatedat)
                VALUES (@Id, @WalletId,@CardNumber, @ExpiryMonth,@ExpiryYear, @CVC, @Status,
                @Currency, @Limit, @CreatedAt, @UpdatedAt)"; 
            var cmd = new CommandDefinition( commandText: cardSql, 
            parameters: wallet.VirtualCards[maxIndex], transaction: _transaction,
             cancellationToken: ct ); 
            await _dbConnection.ExecuteAsync(cmd);     
        }

        public async Task CreateWallet(Wallet wallet)
        {
            var sql = @$"INSERT INTO {_walletTable}
            (id,userid, availablebalance, isactive, createdat, updatedat, currency,pendingbalance)
            VALUES (@Id, @UserId, @AvailableBalance, @IsActive, @CreatedAt, @UpdatedAt, @Currency,
            @PendingBalance)";
            await _dbConnection.ExecuteAsync(sql, wallet, _transaction); 
        }
    }
}
using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Writes;
using MoneyPipe.Domain.WalletAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Writes
{
    public class WalletWriteRepository(IDbConnection dbConnection,IDbTransaction transaction):IWalletWriteRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly IDbTransaction _transaction = transaction;

        private readonly string _walletTable = "Wallets";

        public async Task CreateWallet(Wallet wallet)
        {
            var sql = @$"INSERT INTO {_walletTable}
            (id,userid, balance, isactive, createdat, updatedat)
            VALUES (@Id, @UserId, @Balance, @IsActive, @CreatedAt, @UpdatedAt)";
            await _dbConnection.ExecuteAsync(sql, wallet, _transaction); 
        }
    }
}
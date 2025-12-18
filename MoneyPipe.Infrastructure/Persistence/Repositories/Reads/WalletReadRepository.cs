using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Reads
{
    public class WalletReadRepository(IDbConnection dbConnection):IWalletReadRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly string _walletTable = "Wallets";

        public  async Task<IEnumerable<Wallet>> GetWallets(UserId userId)
        {
            var sql = @$"SELECT * FROM {_walletTable}
                WHERE userid = @UserId
            ";
            return await _dbConnection.QueryAsync<Wallet>(sql, new { UserId = userId });
        }
    }
}
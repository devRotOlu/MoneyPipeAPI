using System.Data;
using Dapper;
using MoneyPipe.Application.Interfaces.Persistence.Reads;
using MoneyPipe.Domain.UserAggregate.ValueObjects;
using MoneyPipe.Domain.WalletAggregate;
using MoneyPipe.Domain.WalletAggregate.Entities;
using MoneyPipe.Domain.WalletAggregate.ValueObjects;

namespace MoneyPipe.Infrastructure.Persistence.Repositories.Reads
{
    public class WalletReadRepository(IDbConnection dbConnection):IWalletReadRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly string _walletTable = "Wallets";
        private readonly string _virtualAccountTable = "VirtualAccounts";
        private readonly string _virtualCardTable = "VirtualCards";

        public async Task<Wallet?> GetWallet(WalletId id)
        {
            var sql = @$"SELECT * from {_walletTable} where id = @Id;
                SELECT * from {_virtualAccountTable} where walletid = @Id;
                SELECT * from {_virtualCardTable} where walletid = @Id
            ";

            using var multi = await _dbConnection.QueryMultipleAsync(sql,new {Id = id});

            var wallet = await multi.ReadFirstOrDefaultAsync<Wallet>();

            if (wallet is null) return null;

            var virtualAccounts = (await multi.ReadAsync<VirtualAccount>()).ToList();

            wallet.AddVirtualAccounts(virtualAccounts);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            var virtualCards = (await multi.ReadAsync<VirtualCard>()).ToList();

            wallet.AddVirtualCards(virtualCards);

            return wallet;

        }

        public  async Task<IEnumerable<Wallet>> GetWallets(UserId userId)
        {
            var sql = @$"SELECT * FROM {_walletTable}
                WHERE userid = @UserId
            ";
            return await _dbConnection.QueryAsync<Wallet>(sql, new { UserId = userId });
        }
    }
}
namespace MoneyPipe.Infrastructure.Repository
{
    using Dapper;
    using MoneyPipe.Application.Interfaces.IRepository;
    using MoneyPipe.Domain.Entities;
    using System.Data;

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDbConnection dbConnection, IDbTransaction transaction):base(dbConnection,transaction) 
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
        }
    }

}

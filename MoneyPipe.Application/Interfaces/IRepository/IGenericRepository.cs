namespace MoneyPipe.Application.Interfaces.IRepository
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        // Task<int> AddAsync(T entity);
        // Task<int> AddBulkAsync(IEnumerable<T> entities);
        // Task<int> UpdateAsync(T entity);
        // Task<int> DeleteAsync(Guid id);
        // Task<int> BulkUpdateAsync(IEnumerable<T> entities);
    }
}

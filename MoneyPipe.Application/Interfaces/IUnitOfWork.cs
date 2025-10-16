namespace MoneyPipe.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IMovieRepository Movies { get; }
        void CommitAsync();
        void RollbackAsync();
    }
}

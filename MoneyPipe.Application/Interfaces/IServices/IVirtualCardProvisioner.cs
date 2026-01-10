namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IVirtualCardProvisioner
    {
        Task CreateVirtualCardAsync();
    }
}
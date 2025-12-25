using MoneyPipe.Application.Enum;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IVirtualAccountProvisionerFactory
    {
        IVirtualAccountProvisioner Create(VirtualAccountMethod method);
    }
}
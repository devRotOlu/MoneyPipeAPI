using MoneyPipe.Application.Enums;

namespace MoneyPipe.Application.Interfaces.IServices
{
    public interface IVirtualAccountProvisionerFactory
    {
        IVirtualAccountProvisioner Create(VirtualAccountMethod method);
    }
}
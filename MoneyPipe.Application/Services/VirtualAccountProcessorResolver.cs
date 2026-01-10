using MoneyPipe.Application.Enums;
using MoneyPipe.Application.Interfaces.IServices;

namespace MoneyPipe.Application.Services
{
    public sealed class VirtualAccountProcessorResolver
    {
        public VirtualAccountProcessorResolver(IEnumerable<IVirtualAccountProcessor> processors)
        {
            Processors = processors.ToDictionary(p => p.Method);
            ProviderCapabilityRegistry.Add("NGN",VirtualAccountMethod.Korapay);
        }
        private Dictionary<VirtualAccountMethod,IVirtualAccountProcessor> Processors {get;}

        private Dictionary<string,VirtualAccountMethod> ProviderCapabilityRegistry {get;} = [];

        public IVirtualAccountProcessor Resolve(string currency)
        {
            if (!ProviderCapabilityRegistry.TryGetValue(currency.ToUpper(),out var method))
                throw new Exception();
            if (!Processors.TryGetValue(method, out var processor))
                throw new Exception();
            return processor;
        }
    }
}
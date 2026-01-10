using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers
{
    public class KYCStatusHandler : BaseTypeHandler<KYCStatus, string>
    {
        protected override KYCStatus Create(string value)
        {
            return Enum.Parse<KYCStatus>(value);
        }

        protected override string GetValue(KYCStatus strong)
        {
            return strong.ToString();
        }
    }
}
using MoneyPipe.Domain.UserAggregate.Enums;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers
{
    public sealed class IdentityCategoryTypeHandler : BaseTypeHandler<IdentityCategory, string>
    {
        protected override IdentityCategory Create(string value)
        {
            return Enum.Parse<IdentityCategory>(value);
        }

        protected override string GetValue(IdentityCategory strong)
        {
            return strong.ToString();
        }
    }
}
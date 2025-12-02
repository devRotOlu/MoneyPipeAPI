using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.UserAggregate.ValueObjects
{
    public class NotificationId(Guid value) : ValueObject
    {
        public Guid Value { get; } = value;

        internal static NotificationId CreateUnique(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
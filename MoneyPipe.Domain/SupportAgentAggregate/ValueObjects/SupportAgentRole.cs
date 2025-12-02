using MoneyPipe.Domain.Common.Models;

namespace MoneyPipe.Domain.SupportAgentAggregate.ValueObjects
{
    public sealed class SupportAgentRole:ValueObject
    {
        public string Value { get; }

        private SupportAgentRole(string value)
        {
            Value = value;
        }

        public static readonly SupportAgentRole Level1 = new("Level1");
        public static readonly SupportAgentRole Level2 = new("Level2");
        public static readonly SupportAgentRole Supervisor = new("Supervisor");
        public static readonly SupportAgentRole Admin = new("Admin");

        public static SupportAgentRole From(string value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
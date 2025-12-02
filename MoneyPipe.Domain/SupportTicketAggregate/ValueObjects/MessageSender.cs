using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.SupportAgentAggregate.ValueObjects;
using MoneyPipe.Domain.SupportTicketAggregate.Enums;
using MoneyPipe.Domain.UserAggregate.ValueObjects;

namespace MoneyPipe.Domain.SupportTicketAggregate.ValueObjects
{
    public sealed class MessageSender : ValueObject
    {
        public MessageSenderType Type { get; }
        public Guid Value { get; }   // Stores the strongly typed ID value

        private MessageSender(MessageSenderType type, Guid value)
        {
            Type = type;
            Value = value;
        }

        public static MessageSender FromUserId(UserId userId) =>
            new(MessageSenderType.User, userId.Value);

        public static MessageSender FromAgentId(SupportAgentId agentId) =>
            new(MessageSenderType.SupportAgent, agentId.Value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Value;
        }
    }
}
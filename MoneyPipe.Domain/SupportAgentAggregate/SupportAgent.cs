using ErrorOr;
using MoneyPipe.Domain.Common.Errors;
using MoneyPipe.Domain.Common.Models;
using MoneyPipe.Domain.SupportAgentAggregate.ValueObjects;

namespace MoneyPipe.Domain.SupportAgentAggregate
{
    public sealed class SupportAgent: AggregateRoot<SupportAgentId>
    {
        private readonly List<SupportAgentRole> _roles = [];

        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public IReadOnlyList<SupportAgentRole> Roles => _roles.AsReadOnly();

        private SupportAgent():base(SupportAgentId.CreateUnique()) { }

        private SupportAgent(SupportAgentId id, string fullName, string email)
            : base(id)
        {
            FullName = fullName;
            Email = email;
            IsActive = true;
        }

        public static ErrorOr<SupportAgent> Create(string fullName, string email)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors.Add(Errors.SupportAgent.InvalidName);

            if (string.IsNullOrWhiteSpace(email))
                errors.Add(Errors.SupportAgent.InvalidEmail);

            if (errors.Count > 0)
                return errors;

            return new SupportAgent(SupportAgentId.CreateUnique(), fullName, email);
        }

        public ErrorOr<Success> AssignRole(SupportAgentRole role)
        {
            if (_roles.Contains(role)) return Errors.SupportAgent.ConflictingRoles;
            _roles.Add(role);
            return Result.Success;
        }

        public void RemoveRole(SupportAgentRole role) => _roles.Remove(role);
        
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

    }
}
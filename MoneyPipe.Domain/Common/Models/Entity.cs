namespace MoneyPipe.Domain.Common.Models
{
    public abstract class Entity<TId>:IEquatable<Entity<TId>> where TId : notnull
    {
        // Dapper-friendly ctor: does not set Id
        protected Entity() { }

        protected Entity(TId id)
        {
            Id = id;
        }

        public TId Id { get; protected set; } 

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (other.Id == null) return false;
            if (Id.Equals(default(TId)) || other.Id.Equals(default(TId))) return false; 
            return Id.Equals(other.Id);
        }

        public bool Equals(Entity<TId>? other)
        {
            return Equals((object?) other);
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

        public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
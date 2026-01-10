using ErrorOr;

namespace MoneyPipe.Domain.Common.Models
{
    public abstract class BaseId<TId>:ValueObject where TId:BaseId<TId>,new()
    {
        public static ErrorOr<TId> CreateUnique(Guid id)
        {
            if (Guid.Empty == id) 
                return Errors.Errors.IdCreation.InvalidId(nameof(TId));
            
            var instance = new TId(); 
            instance.Initialize(id);
            return instance;
        }

        public Guid Value { get; private set; }

        protected void Initialize(Guid id) => Value = id;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
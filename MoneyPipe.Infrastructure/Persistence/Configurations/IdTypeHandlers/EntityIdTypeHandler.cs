using System.Data;
using Dapper;

namespace MoneyPipe.Infrastructure.Persistence.Configurations.IdTypeHandlers
{
    public abstract class EntityIdTypeHandler<TStrong,TRaw>:SqlMapper.TypeHandler<TStrong>
    {
        protected abstract TStrong Create(TRaw value);
        protected abstract TRaw GetValue(TStrong strong);

        public override TStrong Parse(object value)
        {
            return Create((TRaw)value);
        }

        public override void SetValue(IDbDataParameter parameter, TStrong? strong)
        {
            parameter.Value = strong is not null? GetValue(strong) : DBNull.Value;
        }
    }
}
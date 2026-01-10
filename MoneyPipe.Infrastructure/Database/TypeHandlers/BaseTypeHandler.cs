using System.Data;
using Dapper;

namespace MoneyPipe.Infrastructure.Database.TypeHandlers
{
    public abstract class BaseTypeHandler<TStrong,TRaw>:SqlMapper.TypeHandler<TStrong>
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
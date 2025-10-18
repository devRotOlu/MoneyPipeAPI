using Dapper.FluentMap.Mapping;
using System.Linq.Expressions;
using System.Reflection;

namespace MoneyPipe.Infrastructure
{
    public class EntityMapper<T> : EntityMap<T> where T : class
    {
        public EntityMapper()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, prop);
                var convert = Expression.Convert(propertyAccess, typeof(object));
                var lambda = Expression.Lambda<Func<T, object>>(convert, parameter);

                Map(lambda).ToColumn(prop.Name.ToLowerInvariant());
            }
        }
    }
}

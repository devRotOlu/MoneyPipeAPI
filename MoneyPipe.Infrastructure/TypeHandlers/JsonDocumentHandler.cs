
using Dapper;
using Npgsql;
using System.Data;
using System.Text.Json;
namespace MoneyPipe.Infrastructure.TypeHandlers
{
    public class JsonDocumentHandler : SqlMapper.TypeHandler<JsonDocument>
    {
        public override void SetValue(IDbDataParameter parameter, JsonDocument value)
        {
            // Serialize back to string for PostgreSQL
            parameter.Value = value.RootElement.GetRawText();

            // Tell Npgsql this is JSON 
            if (parameter is NpgsqlParameter npgsqlParameter) npgsqlParameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Json; 
        }

        public override JsonDocument Parse(object value)
        {
            // Parse JSON string into JsonDocument
            return JsonDocument.Parse(value.ToString()!);
        }
    }
}
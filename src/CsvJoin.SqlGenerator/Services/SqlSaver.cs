using CsvJoin.SqlGenerator.Services.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.SqlGenerator.Services
{
    public class SqlSaver : ISqlSaver
    {
        public Task SaveSqlAsync(string sql, StreamWriter writer) =>
            writer.WriteAsync(sql);
    }
}

using CsvJoin.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.Sql
{
    public class SqlSaver : ISqlSaver
    {
        public async Task SaveSqlAsync(string sql, string filepath)
            => await File.WriteAllTextAsync(filepath, sql);
    }
}

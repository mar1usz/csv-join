using CsvJoin.Services.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.Services
{
    public class SqlSaver : ISqlSaver
    {
        public async Task SaveSqlAsync(string sql, string path) =>
            await File.WriteAllTextAsync(path, sql);
    }
}

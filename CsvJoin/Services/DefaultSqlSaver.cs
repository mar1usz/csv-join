using CsvJoin.Services.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.Services
{
    public class DefaultSqlSaver : ISqlSaver
    {
        public async Task SaveSqlAsync(string sql, string filepath) =>
            await File.WriteAllTextAsync(filepath, sql);
    }
}

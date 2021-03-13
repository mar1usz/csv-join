using CsvJoin.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class DefaultSqlSaver : ISqlSaver
    {
        public async Task SaveSqlAsync(string sql, string filepath) =>
            await File.WriteAllTextAsync(filepath, sql);
    }
}

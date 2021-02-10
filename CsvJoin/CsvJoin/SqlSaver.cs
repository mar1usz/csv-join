using System.IO;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class SqlSaver
    {
        public async Task SaveSqlAsync(string sql, string filepath)
            => await File.WriteAllTextAsync(filepath, sql);
    }
}

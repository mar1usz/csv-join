using CsvJoin.Services.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.Services
{
    public class SqlReader : ISqlReader
    {
        public Task<string> ReadSqlAsync(string path) =>
            File.ReadAllTextAsync(path);
    }
}

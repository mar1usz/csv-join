using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.Abstractions
{
    public interface ISqlExecutor
    {
        Task ExecuteSqlAsync(string sql, string connectionString, Stream output, CultureInfo culture);
    }
}
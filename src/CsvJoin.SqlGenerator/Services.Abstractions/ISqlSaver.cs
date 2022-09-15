using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.SqlGenerator.Services.Abstractions
{
    public interface ISqlSaver
    {
        Task SaveSqlAsync(string sql, StreamWriter writer);
    }
}

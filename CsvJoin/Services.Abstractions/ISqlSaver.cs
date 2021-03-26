using System.Threading.Tasks;

namespace CsvJoin.Services.Abstractions
{
    public interface ISqlSaver
    {
        Task SaveSqlAsync(string sql, string filePath);
    }
}

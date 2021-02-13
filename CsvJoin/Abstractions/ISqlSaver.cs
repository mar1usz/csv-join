using System.Threading.Tasks;

namespace CsvJoin.Abstractions
{
    public interface ISqlSaver
    {
        Task SaveSqlAsync(string sql, string filepath);
    }
}

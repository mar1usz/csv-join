using System.Threading.Tasks;

namespace CsvJoin.Services.Abstractions
{
    public interface ISqlReader
    {
        Task<string> ReadSqlAsync(string path);
    }
}

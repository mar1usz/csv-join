using System.Globalization;

namespace CsvJoin.Services.Abstractions
{
    public interface ISqlPreparator
    {
        string PrepareFullJoinSql(string directory, string[] filenames, CultureInfo culture);
        string PrepareLeftJoinSql(string directory, string[] filenames, CultureInfo culture);
        string PrepareRightAntiJoinSql(string directory, string[] filenames, CultureInfo culture);
    }
}

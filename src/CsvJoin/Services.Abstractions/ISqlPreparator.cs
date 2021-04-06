using System.Globalization;

namespace CsvJoin.Services.Abstractions
{
    public interface ISqlPreparator
    {
        string PrepareFullJoinSql(string directory, string[] fileNames, CultureInfo culture);
        string PrepareLeftJoinSql(string directory, string[] fileNames, CultureInfo culture);
        string PrepareRightAntiJoinSql(string directory, string[] fileNames, CultureInfo culture);
    }
}

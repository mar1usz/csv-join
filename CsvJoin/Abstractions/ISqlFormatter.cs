namespace CsvJoin.Abstractions
{
    public interface ISqlFormatter
    {
        string FormatSql(string sql, bool insertFinalNewLine = false);
    }
}
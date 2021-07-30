namespace CsvJoin.Services.Abstractions
{
    public interface ISqlFormatter
    {
        string FormatSql(string sql, bool insertFinalNewLine = false);
    }
}
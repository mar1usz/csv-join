namespace CsvJoin.Services.Abstractions
{
    public interface ISqlFormatter
    {
        string FormatSql(string sql, char indentChar = ' ', bool insertFinalNewLine = false);
    }
}
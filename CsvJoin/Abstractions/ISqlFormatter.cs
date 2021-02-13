namespace CsvJoin.Abstractions
{
    public interface ISqlFormatter
    {
        string FormatSql(string sql, char indentChar, bool insertFinalNewLine = false);
    }
}
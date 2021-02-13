using CsvJoin.Abstractions;
using System;

namespace CsvJoin
{
    public class SqlFormatter : ISqlFormatter
    {
        public string FormatSql(
            string sql,
            bool insertFinalNewLine = false)
        {
            if (insertFinalNewLine)
            {
                sql += Environment.NewLine;
            }

            return sql;
        }
    }
}

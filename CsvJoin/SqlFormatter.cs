using CsvJoin.Abstractions;
using System;
using System.Linq;

namespace CsvJoin
{
    public class SqlFormatter : ISqlFormatter
    {
        public string FormatSql(
            string sql,
            bool insertFinalNewLine = false)
        {
            var sqlLinesTrimmed = sql
                .Split(Environment.NewLine)
                .Select(sqlLine => sqlLine.Trim());

            sql = string.Join(Environment.NewLine, sqlLinesTrimmed);

            if (insertFinalNewLine)
            {
                sql += Environment.NewLine;
            }

            return sql;
        }
    }
}

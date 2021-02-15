using CsvJoin.Abstractions;
using CsvJoin.Extensions;
using System;
using System.Linq;

namespace CsvJoin
{
    public class SqlFormatter : ISqlFormatter
    {
        public string FormatSql(
            string sql,
            char indentChar,
            bool insertFinalNewLine = false)
        {
            var sqlLines = sql
                .Split(Environment.NewLine)
                .Select(sqlLine => sqlLine.Trim());

            int indentSizeMax = sqlLines
                .Select(sqlLine => GetIndentSizeFromSqlLine(sqlLine))
                .Max();

            sqlLines = sqlLines
                .Select(sqlLine =>
                    sqlLine.Indent(
                        indentChar,
                        (indentSizeMax - GetIndentSizeFromSqlLine(sqlLine))));

            sql = string.Join(Environment.NewLine, sqlLines);

            if (insertFinalNewLine)
            {
                sql += Environment.NewLine;
            }

            return sql;
        }

        // Gets the index of the first occurence of the '['
        // char or where it would have been if the line
        // doesn't have it.
        private int GetIndentSizeFromSqlLine(string sqlLine) =>
            sqlLine.Contains('[') ? sqlLine.IndexOf('[') : sqlLine.Length + 1;
    }
}

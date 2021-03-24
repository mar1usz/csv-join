using CsvJoin.Extensions;
using CsvJoin.Services.Abstractions;
using System;
using System.Linq;

namespace CsvJoin.Services
{
    public class SqlFormatter : ISqlFormatter
    {
        public string FormatSql(
            string sql,
            char indentChar,
            bool insertFinalNewLine = false)
        {
            string[] sqlLines = sql.Split(Environment.NewLine);

            int indentSizeMax = sqlLines.Max(sqlLine =>
                GetIndentSizeFromSqlLine(sqlLine));

            sqlLines = sqlLines
                .Select(sqlLine => sqlLine.Indent(
                    indentSizeMax - GetIndentSizeFromSqlLine(sqlLine),
                    indentChar))
                .ToArray();

            sql = string.Join(Environment.NewLine, sqlLines);

            if (insertFinalNewLine)
            {
                sql += Environment.NewLine;
            }

            return sql;
        }

        // Gets the index of the first occurence of the '['
        // character or where it would have been if the
        // line doesn't have it.
        private int GetIndentSizeFromSqlLine(string sqlLine) =>
            sqlLine.Contains('[') ? sqlLine.IndexOf('[') : sqlLine.Length + 1;
    }
}

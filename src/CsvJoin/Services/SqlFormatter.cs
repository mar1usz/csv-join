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
            sql = IndentLinesBySquareBracket(sql, indentChar);

            if (insertFinalNewLine)
            {
                sql += Environment.NewLine;
            }

            return sql;
        }

        private string IndentLinesBySquareBracket(string sql, char indentChar)
        {
            string[] sqlLines = sql.Split(Environment.NewLine);

            int indexOfSquareBracketMax = sqlLines.Max(
                sqlLine => GetIndexOfSquareBracket(sqlLine));

            sqlLines = sqlLines
                .Select(sqlLine => sqlLine.Indent(
                    indexOfSquareBracketMax - GetIndexOfSquareBracket(sqlLine),
                    indentChar))
                .ToArray();

            return string.Join(Environment.NewLine, sqlLines);
        }

        private int GetIndexOfSquareBracket(string sqlLine)
        {
            return sqlLine.Contains('[') ? sqlLine.IndexOf('[')
                : sqlLine.Length + 1;
        }
    }
}

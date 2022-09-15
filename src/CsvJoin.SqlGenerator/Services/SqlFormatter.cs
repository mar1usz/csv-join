using CsvJoin.SqlGenerator.Services.Abstractions;
using CsvJoin.SqlGenerator.Extensions;
using System;
using System.Linq;

namespace CsvJoin.SqlGenerator.Services
{
    public class SqlFormatter : ISqlFormatter
    {
        private const char SquareBracket = '[';

        public string FormatSql(
            string sql,
            char indentChar = ' ',
            bool insertFinalNewLine = false)
        {
            sql = AlignLinesBySquareBracket(sql, indentChar);

            if (insertFinalNewLine)
            {
                sql = InsertFinalNewLine(sql);
            }

            return sql;
        }

        private string AlignLinesBySquareBracket(string sql, char indentChar)
        {
            string[] sqlLines = sql.Split(Environment.NewLine);

            int indexOfSquareBracketMax = sqlLines.Max(
                l => GetIndexOfSquareBracket(l));

            sqlLines = sqlLines
                .Select(l => l.Indent(
                    indexOfSquareBracketMax - GetIndexOfSquareBracket(l),
                    indentChar))
                .ToArray();

            return string.Join(Environment.NewLine, sqlLines);
        }

        private int GetIndexOfSquareBracket(string sqlLine)
        {
            return sqlLine.Contains(SquareBracket)
                ? sqlLine.IndexOf(SquareBracket)
                : sqlLine.Length + 1;
        }

        private string InsertFinalNewLine(string sql) =>
            sql + Environment.NewLine;
    }
}

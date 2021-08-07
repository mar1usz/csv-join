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

            int indexOfSquareBracketMax = sqlLines
                .Max(l => GetIndexOfSquareBracket(l));

            sqlLines = sqlLines
                .Select(l => l.Indent(
                    indexOfSquareBracketMax - GetIndexOfSquareBracket(l),
                    indentChar))
                .ToArray();

            return string.Join(Environment.NewLine, sqlLines);
        }

        private string InsertFinalNewLine(string sql) =>
            sql + Environment.NewLine;

        private int GetIndexOfSquareBracket(string sqlLine) =>
            sqlLine.Contains('[') ? sqlLine.IndexOf('[') : sqlLine.Length + 1;
    }
}

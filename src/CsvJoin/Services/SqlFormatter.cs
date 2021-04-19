using CsvJoin.Extensions;
using CsvJoin.Services.Abstractions;
using System;
using System.Collections.Generic;
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
            var sqlLines = sql.Split(Environment.NewLine).AsEnumerable();

            IndentLinesBySquareBracket(sqlLines, indentChar);

            if (insertFinalNewLine)
            {
                sqlLines = sqlLines.Append(string.Empty);
            }

            return string.Join(Environment.NewLine, sqlLines);
        }

        private void IndentLinesBySquareBracket(IEnumerable<string> sqlLines,
            char indentChar)
        {
            int indexOfSquareBracketMax = sqlLines.Max(
                sqlLine => GetIndexOfSquareBracket(sqlLine));

            sqlLines = sqlLines.Select(
                sqlLine => sqlLine.Indent(
                    indexOfSquareBracketMax - GetIndexOfSquareBracket(sqlLine),
                    indentChar));
        }

        private int GetIndexOfSquareBracket(string sqlLine)
        {
            return sqlLine.Contains('[') ? sqlLine.IndexOf('[')
                : sqlLine.Length + 1;
        }
    }
}

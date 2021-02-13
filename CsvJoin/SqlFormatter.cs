﻿using CsvJoin.Abstractions;
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

            sql = string.Join(
                Environment.NewLine,
                sqlLines);

            if (insertFinalNewLine)
            {
                sql += Environment.NewLine;
            }

            return sql;
        }

        private int GetIndentSizeFromSqlLine(string line)
            => (line.Contains('['))
            ? line.IndexOf('[')
            : line.Length + 1;
    }
}

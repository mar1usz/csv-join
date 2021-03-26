using CsvJoin.Services.Abstractions;
using CsvJoin.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvJoin.Services
{
    public class SqlPreparator : ISqlPreparator
    {
        public string PrepareFullJoinSql(
            string directory,
            string[] fileNames,
            CultureInfo culture)
        {
            string sql = "";

            sql += PrepareLeftJoinSql(
                directory,
                fileNames,
                culture);

            sql += Environment.NewLine;
            sql += "UNION";

            sql += Environment.NewLine;
            sql += PrepareRightAntiJoinSql(
                directory,
                fileNames,
                culture);

            return sql;
        }

        public string PrepareLeftJoinSql(
            string directory,
            string[] fileNames,
            CultureInfo culture)
        {
            string[] tables = GetTableNamesFromFileNames(fileNames);
            string[][] columns = GetColumnNamesFromFilePaths(
                directory,
                fileNames,
                culture);

            string[] joinedColumns = columns[0].Union(columns[1]).ToArray();
            string[] commonColumns = columns[0].Intersect(columns[1])
                .ToArray();

            // SELECT _/‗.[]
            //       ,...
            //   FROM _
            string sql = "";

            string joinedColumnsFirst = joinedColumns.First();
            sql += string.Format("SELECT [{0}].[{1}]",
                columns[0].Contains(joinedColumnsFirst) ? tables[0]
                : tables[1],
                joinedColumnsFirst);

            foreach (string joinedColumn in joinedColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(",[{0}].[{1}]",
                    columns[0].Contains(joinedColumn) ? tables[0] : tables[1],
                    joinedColumn);
            }

            sql += Environment.NewLine;
            sql += string.Format("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables[0]);

            // LEFT JOIN ‗ ON _.[] = ‗.[]
            //            AND ...
            sql += Environment.NewLine;
            sql += string.Format("LEFT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables[1]);

            string commonColumnsFirst = commonColumns.First();
            sql += Environment.NewLine;
            sql += string.Format("ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    commonColumn);
            }

            return sql;
        }

        public string PrepareRightAntiJoinSql(
            string directory,
            string[] fileNames,
            CultureInfo culture)
        {
            string[] tables = GetTableNamesFromFileNames(fileNames);
            string[][] columns = GetColumnNamesFromFilePaths(
                directory,
                fileNames,
                culture);

            string[] joinedColumns = columns[0].Union(columns[1]).ToArray();
            string[] commonColumns = columns[0].Intersect(columns[1])
                .ToArray();

            // SELECT _/‗.[]
            //       ,...
            //   FROM _
            string sql = "";

            string joinedColumnsFirst = joinedColumns.First();
            sql += string.Format("SELECT [{0}].[{1}]",
                columns[1].Contains(joinedColumnsFirst) ? tables[1]
                : tables[0],
                joinedColumnsFirst);

            foreach (string joinedColumn in joinedColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(",[{0}].[{1}]",
                    columns[1].Contains(joinedColumn) ? tables[1] : tables[0],
                    joinedColumn);
            }

            sql += Environment.NewLine;
            sql += string.Format("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables[0]);

            // RIGHT JOIN _ ON _.[] = ‗.[]
            //             AND ...
            sql += Environment.NewLine;
            sql += string.Format("RIGHT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables[1]);

            string commonColumnsFirst = commonColumns.First();
            sql += Environment.NewLine;
            sql += string.Format("ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    commonColumn);
            }

            // WHERE _.[] IS NULL
            //   AND ...
            sql += Environment.NewLine;
            sql += string.Format("WHERE [{0}].[{1}] IS NULL",
                tables[0],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format("AND [{0}].[{1}] IS NULL",
                    tables[0],
                    commonColumn);
            }

            return sql;
        }

        private string[] GetTableNamesFromFileNames(string[] fileNames) =>
            fileNames.Select(fileName => Path.GetFileNameWithoutExtension(
                fileName)).ToArray();

        private string[][] GetColumnNamesFromFilePaths(
            string directory,
            string[] fileNames,
            CultureInfo culture) =>
                fileNames.Select(fileName => CsvUtilities.ReadHeader(
                    directory, fileName, culture)).ToArray();
    }
}

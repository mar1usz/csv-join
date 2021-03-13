using CsvJoin.Abstractions;
using CsvJoin.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvJoin
{
    public class SqlPreparator : ISqlPreparator
    {
        public string PrepareFullJoinSql(
            string directory,
            string[] filenames,
            CultureInfo culture)
        {
            string sql = "";

            sql += PrepareLeftJoinSql(
                directory,
                filenames,
                culture);

            sql += Environment.NewLine;
            sql += @"UNION";

            sql += Environment.NewLine;
            sql += PrepareRightAntiJoinSql(
                directory,
                filenames,
                culture);

            return sql;
        }

        public string PrepareLeftJoinSql(
            string directory,
            string[] filenames,
            CultureInfo culture)
        {
            string[] tables = GetTableNamesFromFilenames(filenames);

            string[][] columns = GetColumnNamesFromFilepaths(
                directory,
                filenames,
                culture);

            string[] joinedColumns = columns[0]
                .Union(columns[1])
                .ToArray();

            string[] commonColumns = columns[0]
                .Intersect(columns[1])
                .ToArray();

            // SELECT _/‗.[]
            //       ,_/‗.[]
            //       ,...
            //   FROM _
            string sql = "";

            string joinedColumnsFirst = joinedColumns.First();
            sql += string.Format(@"SELECT [{0}].[{1}]",
                columns[0].Contains(joinedColumnsFirst) ? tables[0] : tables[1],
                joinedColumnsFirst);

            foreach (string joinedColumn in joinedColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(@",[{0}].[{1}]",
                    columns[0].Contains(joinedColumn) ? tables[0] : tables[1],
                    joinedColumn);
            }

            sql += Environment.NewLine;
            sql += string.Format(@"FROM [{0}] AS [{1}]",
                filenames[0],
                tables[0]);

            // LEFT JOIN ‗ ON _.[] = ‗.[]
            //            AND _.[] = ‗.[]
            //            AND ...
            sql += Environment.NewLine;
            sql += string.Format(@"LEFT JOIN [{0}] AS [{1}]",
                filenames[1],
                tables[1]);

            string commonColumnsFirst = commonColumns.First();
            sql += Environment.NewLine;
            sql += string.Format(@"ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(@"AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    commonColumn);
            }

            return sql;
        }

        public string PrepareRightAntiJoinSql(
            string directory,
            string[] filenames,
            CultureInfo culture)
        {
            string[] tables = GetTableNamesFromFilenames(filenames);

            string[][] columns = GetColumnNamesFromFilepaths(
                directory,
                filenames,
                culture);

            string[] joinedColumns = columns[0]
                .Union(columns[1])
                .ToArray();

            string[] commonColumns = columns[0]
                .Intersect(columns[1])
                .ToArray();

            // SELECT _/‗.[]
            //       ,_/‗.[]
            //       ,...
            //   FROM _
            string sql = "";

            string joinedColumnsFirst = joinedColumns.First();
            sql += string.Format(@"SELECT [{0}].[{1}]",
                columns[1].Contains(joinedColumnsFirst) ? tables[1] : tables[0],
                joinedColumnsFirst);

            foreach (string joinedColumn in joinedColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(@",[{0}].[{1}]",
                    columns[1].Contains(joinedColumn) ? tables[1] : tables[0],
                    joinedColumn);
            }

            sql += Environment.NewLine;
            sql += string.Format(@"FROM [{0}] AS [{1}]",
                filenames[0],
                tables[0]);

            // RIGHT JOIN _ ON _.[] = ‗.[]
            //             AND _.[] = ‗.[]
            //             AND ...
            sql += Environment.NewLine;
            sql += string.Format(@"RIGHT JOIN [{0}] AS [{1}]",
                filenames[1],
                tables[1]);

            string commonColumnsFirst = commonColumns.First();
            sql += Environment.NewLine;
            sql += string.Format(@"ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(@"AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    commonColumn);
            }

            // WHERE _.[] IS NULL 
            //   AND _.[] IS NULL
            //   AND ...
            sql += Environment.NewLine;
            sql += string.Format(@"WHERE [{0}].[{1}] IS NULL",
                tables[0],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql += Environment.NewLine;
                sql += string.Format(@"AND [{0}].[{1}] IS NULL",
                    tables[0],
                    commonColumn);
            }

            return sql;
        }

        private string[] GetTableNamesFromFilenames(string[] filenames) =>
            filenames
                .Select(filename => Path.GetFileNameWithoutExtension(filename))
                .ToArray();

        // This function is wrote like this for convenience.
        // In most cases you'd use multidimensional arrays over
        // jagged arrays to prevent wasting space (CA1814).
        private string[][] GetColumnNamesFromFilepaths(
            string directory,
            string[] filenames,
            CultureInfo culture) =>
                filenames
                    .Select(filename => CsvUtilities.ReadHeader(
                        directory, filename, culture))
                    .ToArray();
    }
}

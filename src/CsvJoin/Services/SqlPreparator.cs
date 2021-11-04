using CsvJoin.Services.Abstractions;
using CsvJoin.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvJoin.Services
{
    public class SqlPreparator : ISqlPreparator
    {
        public string PrepareFullJoinSql(string directory, string[] fileNames)
        {
            var sql = new StringBuilder();

            sql.Append(PrepareLeftJoinSql(directory, fileNames));

            sql.AppendLine();
            sql.Append("UNION");

            sql.AppendLine();
            sql.Append(PrepareRightAntiJoinSql(directory, fileNames));

            return sql.ToString();
        }

        public string PrepareLeftJoinSql(string directory, string[] fileNames)
        {
            string[] tables = GetTableNamesFromFileNames(fileNames);
            string[][] columns = GetColumnNamesFromPaths(
                directory, fileNames);

            string[] allColumns = columns[0].Union(columns[1]).ToArray();
            string[] joinColumns = columns[0].Intersect(columns[1]).ToArray();

            var sql = new StringBuilder();

            string allColumnsFirst = allColumns.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                columns[0].Contains(allColumnsFirst) ? tables[0] : tables[1],
                allColumnsFirst);

            foreach (string column in allColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat(",[{0}].[{1}]",
                    columns[0].Contains(column) ? tables[0] : tables[1],
                    column);
            }

            sql.AppendLine();
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables[0]);

            sql.AppendLine();
            sql.AppendFormat("LEFT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables[1]);

            sql.AppendLine();
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                joinColumns.First());

            foreach (string column in joinColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    column);
            }

            return sql.ToString();
        }

        public string PrepareRightAntiJoinSql(
            string directory,
            string[] fileNames)
        {
            string[] tables = GetTableNamesFromFileNames(fileNames);
            string[][] columns = GetColumnNamesFromPaths(
                directory, fileNames);

            string[] allColumns = columns[0].Union(columns[1]).ToArray();
            string[] joinColumns = columns[0].Intersect(columns[1]).ToArray();

            var sql = new StringBuilder();

            string allColumnsFirst = allColumns.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                columns[1].Contains(allColumnsFirst) ? tables[1] : tables[0],
                allColumnsFirst);

            foreach (string column in allColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat(",[{0}].[{1}]",
                    columns[1].Contains(column) ? tables[1] : tables[0],
                    column);
            }

            sql.AppendLine();
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables[0]);

            sql.AppendLine();
            sql.AppendFormat("RIGHT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables[1]);

            string joinColumnsFirst = joinColumns.First();
            sql.AppendLine();
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                joinColumnsFirst);

            foreach (string column in joinColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    column);
            }

            sql.AppendLine();
            sql.AppendFormat("WHERE [{0}].[{1}] IS NULL",
                tables[0],
                joinColumnsFirst);

            foreach (string column in joinColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{1}] IS NULL",
                    tables[0],
                    column);
            }

            return sql.ToString();
        }

        private string[] GetTableNamesFromFileNames(string[] fileNames)
        {
            return fileNames
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .ToArray();
        }

        private string[][] GetColumnNamesFromPaths(
            string directory,
            string[] fileNames)
        {
            return fileNames
                .Select(f => CsvUtilities.ReadHeader(directory, f))
                .ToArray();
        }
    }
}

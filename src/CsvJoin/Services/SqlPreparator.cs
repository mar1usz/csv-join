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

            sql.Append(Environment.NewLine);
            sql.Append("UNION");

            sql.Append(Environment.NewLine);
            sql.Append(PrepareRightAntiJoinSql(directory, fileNames));

            return sql.ToString();
        }

        public string PrepareLeftJoinSql(string directory, string[] fileNames)
        {
            string[] tables = GetTableNamesFromFileNames(fileNames);
            string[][] columns = GetColumnNamesFromFilePaths(
                directory, fileNames);

            string[] joinedColumns = columns[0].Union(columns[1]).ToArray();
            string[] commonColumns = columns[0].Intersect(columns[1])
                .ToArray();

            var sql = new StringBuilder();

            string joinedColumnsFirst = joinedColumns.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                columns[0].Contains(joinedColumnsFirst) ? tables[0]
                    : tables[1],
                joinedColumnsFirst);

            foreach (string joinedColumn in joinedColumns.Skip(1))
            {
                sql.Append(Environment.NewLine);
                sql.AppendFormat(",[{0}].[{1}]",
                    columns[0].Contains(joinedColumn) ? tables[0] : tables[1],
                    joinedColumn);
            }

            sql.Append(Environment.NewLine);
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables[0]);

            sql.Append(Environment.NewLine);
            sql.AppendFormat("LEFT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables[1]);

            sql.Append(Environment.NewLine);
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                commonColumns.First());

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql.Append(Environment.NewLine);
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    commonColumn);
            }

            return sql.ToString();
        }

        public string PrepareRightAntiJoinSql(
            string directory, string[] fileNames)
        {
            string[] tables = GetTableNamesFromFileNames(fileNames);
            string[][] columns = GetColumnNamesFromFilePaths(
                directory, fileNames);

            string[] joinedColumns = columns[0].Union(columns[1]).ToArray();
            string[] commonColumns = columns[0].Intersect(columns[1])
                .ToArray();

            var sql = new StringBuilder();

            string joinedColumnsFirst = joinedColumns.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                columns[1].Contains(joinedColumnsFirst) ? tables[1]
                    : tables[0],
                joinedColumnsFirst);

            foreach (string joinedColumn in joinedColumns.Skip(1))
            {
                sql.Append(Environment.NewLine);
                sql.AppendFormat(",[{0}].[{1}]",
                    columns[1].Contains(joinedColumn) ? tables[1] : tables[0],
                    joinedColumn);
            }

            sql.Append(Environment.NewLine);
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables[0]);

            sql.Append(Environment.NewLine);
            sql.AppendFormat("RIGHT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables[1]);

            string commonColumnsFirst = commonColumns.First();
            sql.Append(Environment.NewLine);
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                tables[0],
                tables[1],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql.Append(Environment.NewLine);
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables[0],
                    tables[1],
                    commonColumn);
            }

            sql.Append(Environment.NewLine);
            sql.AppendFormat("WHERE [{0}].[{1}] IS NULL",
                tables[0],
                commonColumnsFirst);

            foreach (string commonColumn in commonColumns.Skip(1))
            {
                sql.Append(Environment.NewLine);
                sql.AppendFormat("AND [{0}].[{1}] IS NULL",
                    tables[0],
                    commonColumn);
            }

            return sql.ToString();
        }

        private string[] GetTableNamesFromFileNames(string[] fileNames)
        {
            return fileNames
                .Select(fileName => Path.GetFileNameWithoutExtension(fileName))
                .ToArray();
        }

        private string[][] GetColumnNamesFromFilePaths(
            string directory,
            string[] fileNames)
        {
            return fileNames
                .Select(fileName => CsvUtilities.ReadHeader(
                    directory, fileName))
                .ToArray();
        }
    }
}

using CsvJoin.Services.Abstractions;
using CsvJoin.Utilities;
using System;
using System.Collections.Generic;
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
            var tables = GetTables(directory, fileNames);

            var allColumns = GetAllColumns(tables);
            var joinColumns = GetJoinColumns(tables);

            var sql = new StringBuilder();

            var allColumnsFirst = allColumns.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                tables.ElementAt(0).ContainsColumn(allColumnsFirst)
                    ? tables.ElementAt(0).Name
                    : tables.ElementAt(1).Name,
                allColumnsFirst.Name);

            foreach (var column in allColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat(",[{0}].[{1}]",
                    tables.ElementAt(0).ContainsColumn(column)
                        ? tables.ElementAt(0).Name
                        : tables.ElementAt(1).Name,
                    column.Name);
            }

            sql.AppendLine();
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables.ElementAt(0).Name);

            sql.AppendLine();
            sql.AppendFormat("LEFT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables.ElementAt(1).Name);

            sql.AppendLine();
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                tables.ElementAt(0).Name,
                tables.ElementAt(1).Name,
                joinColumns.First().Name);

            foreach (var column in joinColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables.ElementAt(0).Name,
                    tables.ElementAt(1).Name,
                    column.Name);
            }

            return sql.ToString();
        }

        public string PrepareRightAntiJoinSql(
            string directory,
            string[] fileNames)
        {
            var tables = GetTables(directory, fileNames);

            var allColumns = GetAllColumns(tables);
            var joinColumns = GetJoinColumns(tables);

            var sql = new StringBuilder();

            var allColumnsFirst = allColumns.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                tables.ElementAt(1).ContainsColumn(allColumnsFirst)
                    ? tables.ElementAt(1).Name
                    : tables.ElementAt(0).Name,
                allColumnsFirst.Name);

            foreach (var column in allColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat(",[{0}].[{1}]",
                    tables.ElementAt(1).ContainsColumn(column)
                        ? tables.ElementAt(1).Name
                        : tables.ElementAt(0).Name,
                    column.Name);
            }

            sql.AppendLine();
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                fileNames[0],
                tables.ElementAt(0).Name);

            sql.AppendLine();
            sql.AppendFormat("RIGHT JOIN [{0}] AS [{1}]",
                fileNames[1],
                tables.ElementAt(1).Name);

            var joinColumnsFirst = joinColumns.First();
            sql.AppendLine();
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                tables.ElementAt(0).Name,
                tables.ElementAt(1).Name,
                joinColumnsFirst.Name);

            foreach (var column in joinColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    tables.ElementAt(0).Name,
                    tables.ElementAt(1).Name,
                    column.Name);
            }

            sql.AppendLine();
            sql.AppendFormat("WHERE [{0}].[{1}] IS NULL",
                tables.ElementAt(0).Name,
                joinColumnsFirst.Name);

            foreach (var column in joinColumns.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{1}] IS NULL",
                    tables.ElementAt(0).Name,
                    column.Name);
            }

            return sql.ToString();
        }

        private IEnumerable<Table> GetTables(
            string directory,
            string[] fileNames)
        {
            var tables = new List<Table> { };
            foreach (var fileName in fileNames)
            {
                tables.Add(new Table
                {
                    Name = GetTableName(fileName),
                    Columns = GetColumnNames(directory, fileName)
                        .Select(f => new Column { Name = f })
                });
            }
            return tables;
        }

        private string GetTableName(string fileName) =>
             Path.GetFileNameWithoutExtension(fileName);

        private IEnumerable<string> GetColumnNames(
            string directory,
            string fileName)
        {
            return CsvUtilities.ReadHeader(directory, fileName);
        }

        IEnumerable<Column> GetAllColumns(IEnumerable<Table> tables) =>
            tables.ElementAt(0).Columns.Union(tables.ElementAt(1).Columns);

        IEnumerable<Column> GetJoinColumns(IEnumerable<Table> tables) =>
            tables.ElementAt(0).Columns.Intersect(tables.ElementAt(1).Columns);

        private class Table
        {
            public string Name { get; set; }
            public IEnumerable<Column> Columns { get; set; }

            public bool ContainsColumn(Column column) =>
                Columns.Contains(column);
        }

        private class Column : IEquatable<Column>
        {
            public string Name { get; set; }

            public bool Equals(Column other) =>
                other != null && Name == other.Name;

            public override bool Equals(object obj) =>
                Equals(obj as Column);

            public override int GetHashCode() =>
                Name.GetHashCode();
        }
    }
}

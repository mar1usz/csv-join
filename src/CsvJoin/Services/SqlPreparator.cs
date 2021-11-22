using CsvJoin.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvJoin.Services
{
    public partial class SqlPreparator : ISqlPreparator
    {
        public string PrepareFullJoinSql(IEnumerable<Table> tables)
        {
            var sql = new StringBuilder();

            sql.Append(PrepareLeftJoinSql(tables));

            sql.AppendLine();
            sql.Append("UNION");

            sql.AppendLine();
            sql.Append(PrepareRightAntiJoinSql(tables));

            return sql.ToString();
        }

        public string PrepareLeftJoinSql(IEnumerable<Table> tables)
        {
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
                tables.ElementAt(0).FileName,
                tables.ElementAt(0).Name);

            sql.AppendLine();
            sql.AppendFormat("LEFT JOIN [{0}] AS [{1}]",
                tables.ElementAt(1).FileName,
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

        public string PrepareRightAntiJoinSql(IEnumerable<Table> tables)
        {
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
                tables.ElementAt(0).FileName,
                tables.ElementAt(0).Name);

            sql.AppendLine();
            sql.AppendFormat("RIGHT JOIN [{0}] AS [{1}]",
                tables.ElementAt(1).FileName,
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

        private IEnumerable<Column> GetAllColumns(IEnumerable<Table> tables) =>
            tables.ElementAt(0).Columns.Union(tables.ElementAt(1).Columns);

        private IEnumerable<Column> GetJoinColumns(IEnumerable<Table> tables) =>
            tables.ElementAt(0).Columns.Intersect(tables.ElementAt(1).Columns);
    }
}

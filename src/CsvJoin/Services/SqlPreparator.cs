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
            var sql = new StringBuilder();

            var command = ExtractCommand(directory, fileNames);

            string allColumnsFirst = command.AllColumnNames.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                command.ColumnNames[0].Contains(allColumnsFirst)
                    ? command.TableNames[0]
                    : command.TableNames[1],
                allColumnsFirst);

            foreach (string column in command.AllColumnNames.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat(",[{0}].[{1}]",
                    command.ColumnNames[0].Contains(column)
                        ? command.TableNames[0]
                        : command.TableNames[1],
                    column);
            }

            sql.AppendLine();
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                command.FileNames[0],
                command.TableNames[0]);

            sql.AppendLine();
            sql.AppendFormat("LEFT JOIN [{0}] AS [{1}]",
                command.FileNames[1],
                command.TableNames[1]);

            sql.AppendLine();
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                command.TableNames[0],
                command.TableNames[1],
                command.JoinColumnNames.First());

            foreach (string column in command.JoinColumnNames.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    command.TableNames[0],
                    command.TableNames[1],
                    column);
            }

            return sql.ToString();
        }

        public string PrepareRightAntiJoinSql(
            string directory,
            string[] fileNames)
        {
            var sql = new StringBuilder();

            var command = ExtractCommand(directory, fileNames);

            string allColumnsFirst = command.AllColumnNames.First();
            sql.AppendFormat("SELECT [{0}].[{1}]",
                command.ColumnNames[1].Contains(allColumnsFirst)
                    ? command.TableNames[1]
                    : command.TableNames[0],
                allColumnsFirst);

            foreach (string column in command.AllColumnNames.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat(",[{0}].[{1}]",
                    command.ColumnNames[1].Contains(column)
                        ? command.TableNames[1]
                        : command.TableNames[0],
                    column);
            }

            sql.AppendLine();
            sql.AppendFormat("FROM [{0}] AS [{1}]",
                command.FileNames[0],
                command.TableNames[0]);

            sql.AppendLine();
            sql.AppendFormat("RIGHT JOIN [{0}] AS [{1}]",
                command.FileNames[1],
                command.TableNames[1]);

            string joinColumnsFirst = command.JoinColumnNames.First();
            sql.AppendLine();
            sql.AppendFormat("ON [{0}].[{2}] = [{1}].[{2}]",
                command.TableNames[0],
                command.TableNames[1],
                joinColumnsFirst);

            foreach (string column in command.JoinColumnNames.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{2}] = [{1}].[{2}]",
                    command.TableNames[0],
                    command.TableNames[1],
                    column);
            }

            sql.AppendLine();
            sql.AppendFormat("WHERE [{0}].[{1}] IS NULL",
                command.TableNames[0],
                joinColumnsFirst);

            foreach (string column in command.JoinColumnNames.Skip(1))
            {
                sql.AppendLine();
                sql.AppendFormat("AND [{0}].[{1}] IS NULL",
                    command.TableNames[0],
                    column);
            }

            return sql.ToString();
        }

        private PrepareJoinCommand ExtractCommand(
            string directory,
            string[] fileNames)
        {
            string[] tableNames = ExtractTableNames(fileNames);
            string[][] columnNames = ExtractColumnNames(directory, fileNames);

            return new PrepareJoinCommand
            {
                FileNames = fileNames,
                TableNames = tableNames,
                ColumnNames = columnNames
            };
        }

        private string[] ExtractTableNames(string[] fileNames)
        {
            return fileNames
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .ToArray();
        }

        private string[][] ExtractColumnNames(
            string directory,
            string[] fileNames)
        {
            return fileNames
                .Select(f => CsvUtilities.ReadHeader(directory, f))
                .ToArray();
        }

        private class PrepareJoinCommand
        {
            public string[] FileNames { get; set; }
            public string[] TableNames { get; set; }
            public string[][] ColumnNames { get; set; }
            public string[] AllColumnNames =>
                ColumnNames[0].Union(ColumnNames[1]).ToArray();
            public string[] JoinColumnNames =>
                ColumnNames[0].Intersect(ColumnNames[1]).ToArray();
        }
    }
}

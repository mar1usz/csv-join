using CsvJoin.Services.Abstractions;
using System;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class Application
    {
        private const string SqlPath = "SQLQuery.sql";

        private static readonly Stream Output = Console.OpenStandardOutput();

        private readonly ITablesExtractor _tablesExtractor;
        private readonly ISqlPreparator _sqlPreparator;
        private readonly ISqlFormatter _sqlFormatter;
        private readonly ISqlExecutor _sqlExecutor;
        private readonly ISqlSaver _sqlSaver;

        public Application(
            ITablesExtractor tablesExtractor,
            ISqlPreparator sqlPreparator,
            ISqlFormatter sqlFormatter,
            ISqlExecutor sqlExecutor,
            ISqlSaver sqlSaver)
        {
            _tablesExtractor = tablesExtractor;
            _sqlPreparator = sqlPreparator;
            _sqlFormatter = sqlFormatter;
            _sqlExecutor = sqlExecutor;
            _sqlSaver = sqlSaver;
        }

        public async Task RunAsync(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException(nameof(args));
            }

            string directory = args.First();
            string[] fileNames = args.Skip(1).Take(2).ToArray();

            var tables = _tablesExtractor.ExtractTables(directory, fileNames);

            string sql = _sqlPreparator.PrepareFullJoinSql(tables);

            sql = _sqlFormatter.FormatSql(sql);

            string connectionString = GetConnectionString(directory);

            await _sqlExecutor.ExecuteSqlAsync(
                sql,
                connectionString,
                Output);

            await _sqlSaver.SaveSqlAsync(sql, SqlPath);
        }

        private string GetConnectionString(string directory)
        {
            var connectionString = new OleDbConnectionStringBuilder
            {
                { "Provider", "Microsoft.ACE.OLEDB.16.0" },
                { "Data Source", directory },
                { "OLE DB Services", -1 },
                { "Extended Properties", "text;Excel 16.0;HDR=YES;IMEX=1" }
            };

            return connectionString.ToString();
        }
    }
}

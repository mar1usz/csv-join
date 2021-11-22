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

        private readonly ITablesExtractor _extractor;
        private readonly ISqlPreparator _preparator;
        private readonly ISqlFormatter _formatter;
        private readonly ISqlExecutor _executor;
        private readonly ISqlSaver _saver;

        public Application(
            ITablesExtractor extractor,
            ISqlPreparator preparator,
            ISqlFormatter formatter,
            ISqlExecutor executor,
            ISqlSaver saver)
        {
            _extractor = extractor;
            _preparator = preparator;
            _formatter = formatter;
            _executor = executor;
            _saver = saver;
        }

        public async Task RunAsync(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException(nameof(args));
            }

            string directory = args.First();
            string[] fileNames = args.Skip(1).Take(2).ToArray();

            var tables = _extractor.ExtractTables(directory, fileNames);

            string sql = _preparator.PrepareFullJoinSql(tables);

            sql = _formatter.FormatSql(sql);

            string connectionString = GetConnectionString(directory);

            await _executor.ExecuteSqlAsync(
                sql,
                connectionString,
                Output);

            await _saver.SaveSqlAsync(sql, SqlPath);
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

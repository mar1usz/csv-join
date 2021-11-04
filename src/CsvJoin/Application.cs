using CsvJoin.Extensions;
using CsvJoin.Services.Abstractions;
using System;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class Application
    {
        private const string SqlSaverFilePath = "SQLQuery.sql";

        private readonly ISqlPreparator _preparator;
        private readonly ISqlFormatter _formatter;
        private readonly ISqlExecutor _executor;
        private readonly ISqlSaver _saver;

        public Application(
            ISqlPreparator preparator,
            ISqlFormatter formatter,
            ISqlExecutor executor,
            ISqlSaver saver)
        {
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

            string sql = _preparator.PrepareFullJoinSql(directory, fileNames);

            sql = _formatter.FormatSql(sql);

            string connectionString = GetConnectionString(directory);

            var output = Console.OpenStandardOutput();

            await _executor.ExecuteSqlAsync(
                sql,
                connectionString,
                output);

            await _saver.SaveSqlAsync(sql, filePath: SqlSaverFilePath);
        }

        private static string GetConnectionString(string directory)
        {
            var connectionString = new OleDbConnectionStringBuilder();

            connectionString.AddProvider("Microsoft.ACE.OLEDB.16.0");
            connectionString.AddDataSource(directory);
            connectionString.AddOleDbServices("-1");
            connectionString.AddExtendedProperties("text;Excel 16.0;HDR=YES;IMEX=1");

            return connectionString.ToString();
        }
    }
}

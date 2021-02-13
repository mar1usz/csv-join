using CsvJoin.Abstractions;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class Application
    {
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

            string[] filenames = args.Skip(1).Take(2).ToArray();

            var culture = CultureInfo.InvariantCulture;

            string sql = _preparator.PrepareFullJoinSql(
                directory,
                filenames,
                culture);

            sql = _formatter.FormatSql(sql);


            string connectionString = $@"Provider=Microsoft.ACE.OLEDB.16.0;
                Data Source={directory};
                OLE DB Services=-1;
                Extended Properties=""text;Excel 16.0;HDR=YES;IMEX=1""";

            var output = Console.OpenStandardOutput();

            await _executor.ExecuteSqlAsync(
                sql,
                connectionString,
                output,
                culture);

            await _saver.SaveSqlAsync(sql, filepath: @"SQLQuery.sql");
        }
    }
}

using CsvJoin.SqlGenerator.Services.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin.SqlGenerator
{
    public class Application
    {
        private static readonly Stream Output = Console.OpenStandardOutput();

        private readonly ISqlPreparator _preparator;
        private readonly ISqlFormatter _formatter;
        private readonly ISqlSaver _saver;

        public Application(
            ISqlPreparator preparator,
            ISqlFormatter formatter,
            ISqlSaver saver)
        {
            _preparator = preparator;
            _formatter = formatter;
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

            using var writer = new StreamWriter(Output);
            await _saver.SaveSqlAsync(sql, writer);
        }
    }
}

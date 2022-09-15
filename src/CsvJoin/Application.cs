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
        private static readonly Stream Output =
            Console.OpenStandardOutput();

        private readonly ISqlReader _reader;
        private readonly ISqlExecutor _executor;

        public Application(
            ISqlReader reader,
            ISqlExecutor executor)
        {
            _reader = reader;
            _executor = executor;
        }

        public async Task RunAsync(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException(nameof(args));
            }

            string sqlPath = args.First();
            string csvFilesDirectory = args.Skip(1).First();

            string sql = await _reader.ReadSqlAsync(sqlPath);
            string connectionString = GetConnectionString(csvFilesDirectory);

            await _executor.ExecuteSqlAsync(
                sql,
                connectionString,
                Output);
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

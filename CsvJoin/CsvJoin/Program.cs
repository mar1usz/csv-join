using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class Program
    {
        private static readonly SqlPreparator _preparator = new SqlPreparator();
        private static readonly SqlExecutor _executor = new SqlExecutor();
        private static readonly SqlSaver _saver = new SqlSaver();

        public static async Task Main(string[] args)
        {
            string directory = args.First();

            string[] filenames = args.Skip(1).Take(2).ToArray();

            var culture = CultureInfo.InvariantCulture;

            string sql = _preparator.PrepareFullJoinSql(directory, filenames,
                culture);


            string connectionString = $@"Provider=Microsoft.ACE.OLEDB.16.0;
                Data Source={directory};
                OLE DB Services=-1;
                Extended Properties=""text;Excel 16.0;HDR=YES;IMEX=1""";

            var output = Console.OpenStandardOutput();

            await _executor.ExecuteSqlAsync(sql, connectionString, output, culture);

            await _saver.SaveSqlAsync(sql, filepath: @"SQLQuery.sql");
        }
    }
}

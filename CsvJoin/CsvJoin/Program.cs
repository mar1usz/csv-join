using CsvHelper;
using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string directory = args.First();

            string[] filenames = args
                .Skip(1)
                .Select(filepath => Path.GetFileName(filepath))
                .ToArray();

            var culture = CultureInfo.InvariantCulture;

            SqlPreparator preparator = new SqlPreparator();

            string sql = preparator.PrepareFullJoinSql(directory, filenames,
                culture);


            string connectionString = $@"Provider=Microsoft.ACE.OLEDB.16.0;
                Data Source={directory};
                OLE DB Services=-1;
                Extended Properties=""text;Excel 16.0;HDR=YES;IMEX=1""";

            var output = Console.OpenStandardOutput();

            await ExecuteSqlRawAsync(sql, connectionString, output, culture);

            await SaveSqlRawAsync(sql, filepath: @"SQLQuery.sql");
        }

        public static async Task ExecuteSqlRawAsync(string sql,
            string connectionString, Stream output, CultureInfo culture)
        {
            using var connection = new OleDbConnection(connectionString);

            connection.Open();

            var command = new OleDbCommand(sql, connection);

            var reader = await command.ExecuteReaderAsync();


            using var writer = new StreamWriter(output);
            using var csv = new CsvWriter(writer, culture);

            var cols = reader.GetColumnSchema();

            for (int i = 0; i < cols.Count; i++)
            {
                csv.WriteField(cols[i].ColumnName);
            }
            csv.NextRecord();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    csv.WriteField(reader[i]);
                }
                csv.NextRecord();
            }
        }

        public static async Task SaveSqlRawAsync(string sql, string filepath)
            => await File.WriteAllTextAsync(filepath, sql);
    }
}

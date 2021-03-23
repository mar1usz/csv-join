using CsvHelper;
using CsvJoin.Services.Abstractions;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace CsvJoin.Services
{
    public class SqlExecutor : ISqlExecutor
    {
        public async Task ExecuteSqlAsync(
            string sql,
            string connectionString,
            Stream output,
            CultureInfo culture)
        {
            using var connection = new OleDbConnection(connectionString);
            var command = new OleDbCommand(sql, connection);
            connection.Open();
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
            reader.Close();
        }
    }
}

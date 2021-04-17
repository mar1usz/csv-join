using CsvJoin.Services.Abstractions;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvJoin.Services
{
    public class SqlExecutor : ISqlExecutor
    {
        public async Task ExecuteSqlAsync(
            string sql,
            string connectionString,
            Stream output)
        {
            using var connection = new OleDbConnection(connectionString);
            var command = new OleDbCommand(sql, connection);
            connection.Open();

            using var reader = await command.ExecuteReaderAsync();
            WriteResultsToCsv(reader, output);
        }

        private void WriteResultsToCsv(DbDataReader reader, Stream output)
        {
            var header = reader
                .GetColumnSchema()
                .Select(c => c.ColumnName);
            CsvSerializer.SerializeToStream(header, output);

            var record = new List<string> { };
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    record.Add(reader[i].ToString());
                }

                CsvSerializer.SerializeToStream(record, output);
                record.Clear();
            }
        }
    }
}

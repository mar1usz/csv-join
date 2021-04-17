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
            var header = GetHeader(reader);
            CsvSerializer.SerializeToStream(header, output);

            while (reader.Read())
            {
                var record = GetRecord(reader);
                CsvSerializer.SerializeToStream(record, output);
            }
        }

        private static IEnumerable<string> GetHeader(DbDataReader reader)
        {
            return reader
                .GetColumnSchema()
                .Select(c => c.ColumnName);
        }

        private IEnumerable<string> GetRecord(DbDataReader reader)
        {
            var record = new List<string> { };
            for (int i = 0; i < reader.FieldCount; i++)
            {
                record.Add(reader[i].ToString());
            }
            return record;
        }
    }
}

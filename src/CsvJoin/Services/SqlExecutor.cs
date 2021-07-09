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
            TextWriter output)
        {
            using var connection = new OleDbConnection(connectionString);
            using var command = new OleDbCommand(sql, connection);
            connection.Open();

            using var reader = await command.ExecuteReaderAsync();
            WriteResultsToCsv(reader, output);
        }

        private void WriteResultsToCsv(DbDataReader reader, TextWriter output)
        {
            var header = GetHeader(reader);
            CsvSerializer.SerializeToWriter(header, output);

            while (reader.Read())
            {
                var record = GetRecord(reader);
                CsvSerializer.SerializeToWriter(record, output);
            }
        }

        private IEnumerable<string> GetHeader(DbDataReader reader) =>
            reader.GetColumnSchema().Select(c => c.ColumnName);

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

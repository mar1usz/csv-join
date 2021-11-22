using CsvJoin.Services.Abstractions;
using CsvJoin.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvJoin.Services
{
    public class TablesExtractor : ITablesExtractor
    {
        public IEnumerable<Table> ExtractTables(
            string directory,
            string[] fileNames)
        {
            var tables = new List<Table> { };

            foreach (var fileName in fileNames)
            {
                tables.Add(new Table
                {
                    Name = GetTableName(fileName),
                    FileName = fileName,
                    Columns = ExtractColumns(directory, fileName)
                });
            }

            return tables;
        }

        private IEnumerable<Column> ExtractColumns(
            string directory,
            string fileName)
        {
            return GetColumnNames(directory, fileName)
                .Select(cn => new Column { Name = cn });
        }

        private string GetTableName(string fileName) =>
             Path.GetFileNameWithoutExtension(fileName);

        private IEnumerable<string> GetColumnNames(
            string directory,
            string fileName)
        {
            return CsvUtilities.ReadHeader(directory, fileName);
        }
    }
}

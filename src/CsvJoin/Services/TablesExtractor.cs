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
                    Columns = GetColumnNames(directory, fileName)
                        .Select(f => new Column { Name = f })
                });
            }

            return tables;
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

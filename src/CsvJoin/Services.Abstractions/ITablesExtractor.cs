using System.Collections.Generic;

namespace CsvJoin.Services.Abstractions
{
    public interface ITablesExtractor
    {
        IEnumerable<Table> ExtractTables(string directory, string[] fileNames);
    }
}
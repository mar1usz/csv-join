using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvJoin
{
    public class Table
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public IEnumerable<Column> Columns { get; set; }

        public bool ContainsColumn(Column column) =>
            Columns.Contains(column);
    }
}

using System;

namespace CsvJoin.Services
{
    public class Column : IEquatable<Column>
    {
        public string Name { get; set; }

        public bool Equals(Column other) =>
            other != null && Name == other.Name;

        public override bool Equals(object obj) =>
            Equals(obj as Column);

        public override int GetHashCode() =>
            Name.GetHashCode();
    }
}

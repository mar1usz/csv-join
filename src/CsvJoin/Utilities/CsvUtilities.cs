using ServiceStack.Text;
using System.IO;
using System.Linq;

namespace CsvJoin.Utilities
{
    public static class CsvUtilities
    {
        public static string[] ReadHeader(string directory, string fileName)
        {
            var lines = File.ReadLines(@$"{directory}\{fileName}");
            return CsvReader.ParseFields(lines.First()).ToArray();
        }
    }
}
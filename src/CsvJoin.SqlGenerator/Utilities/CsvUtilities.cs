using ServiceStack.Text;
using System.IO;
using System.Linq;

namespace CsvJoin.SqlGenerator.Utilities
{
    public static class CsvUtilities
    {
        public static string[] ReadHeader(string directory, string fileName)
        {
            string path = Path.Join(directory, fileName);

            string header = File.ReadLines(path).First();
            return CsvReader.ParseFields(header).ToArray();
        }
    }
}

using ServiceStack.Text;
using System;
using System.IO;
using System.Linq;

namespace CsvJoin.Utilities
{
    public static class CsvUtilities
    {
        public static string[] ReadHeader(string directory, string fileName)
        {
            string path = Path.Combine(
                Environment.CurrentDirectory,
                directory,
                fileName);

            string header = File.ReadLines(path).First();
            return CsvReader.ParseFields(header).ToArray();
        }
    }
}

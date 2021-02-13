using CsvHelper;
using System.Globalization;
using System.IO;

namespace CsvJoin.Utilities
{
    public static class CsvUtilities
    {
        public static string[] ReadHeader(
            string directory,
            string filename,
            CultureInfo culture)
        {
            using var reader = new StreamReader($@"{directory}\{filename}");
            using var csv = new CsvReader(reader, culture);

            csv.Read();
            csv.ReadHeader();

            return csv.HeaderRecord;
        }
    }
}

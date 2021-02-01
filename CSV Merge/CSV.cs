using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CSV_Merge
{
    public static class CSV
    {
        public static void Main(string[] args)
        {
            Stream output = Console.OpenStandardOutput();
            CultureInfo culture = CultureInfo.InvariantCulture;
            Merge(args, output, culture);
        }

        public static void Merge(string[] filepaths, Stream output, CultureInfo culture)
        {
            if (filepaths.Length < 2)
            {
                throw new ArgumentNullException(nameof(filepaths));
            }

            // Read the first CSV
            DataTable dt = ReadCsv(filepaths.First(), culture);

            // Read and merge the rest of CSVs
            foreach (string filepath in filepaths.Skip(1))
            {
                DataTable dtAnother = ReadCsv(filepath, culture);
                MergeDataTables(dt, dtAnother);
            }

            // Write merged CSV to the output
            WriteCsv(dt, output, culture);
        }

        private static void MergeDataTables(DataTable dt1, DataTable dt2)
        {
            dt1.PrimaryKey = dt1.Columns.Cast<DataColumn>().Where(dt1col => dt2.Columns.Contains(dt1col.ColumnName)).ToArray();
            dt2.PrimaryKey = dt2.Columns.Cast<DataColumn>().Where(dt2col => dt1.Columns.Contains(dt2col.ColumnName)).ToArray();

            dt1.Merge(dt2);
        }

        private static DataTable ReadCsv(string filepath, CultureInfo culture)
        {
            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, culture))
            {
                csv.Configuration.TrimOptions = TrimOptions.Trim;

                using (var dr = new CsvDataReader(csv))
                {
                    DataTable dt = new DataTable();

                    dt.Load(dr);

                    return dt;
                }
            }
        }

        private static void WriteCsv(DataTable dt, Stream output, CultureInfo culture)
        {
            using (var writer = new StreamWriter(output))
            using (var csv = new CsvWriter(writer, culture))
            {
                csv.Configuration.TrimOptions = TrimOptions.Trim;

                int cols = dt.Columns.Count;
                int rows = dt.Rows.Count;

                for (int i = 0; i < cols; i++)
                {
                    csv.WriteField(dt.Columns[i]);
                }
                csv.NextRecord();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        csv.WriteField(dt.Rows[i][dt.Columns[j]]);
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}

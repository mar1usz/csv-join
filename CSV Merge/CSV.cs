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

            // Read the first csv
            DataTable dt = ReadCsv(filepaths.First(), culture);

            // Read and merge the rest of csvs
            foreach (string filepath in filepaths.Skip(1))
            {
                DataTable anotherDt = ReadCsv(filepath, culture);
                MergeDataTables(dt, anotherDt);
            }

            // Write merged csv to output
            WriteCsv(dt, output, culture);
        }

        private static void MergeDataTables(DataTable dt1, DataTable dt2)
        {
            DataColumn[] dt1PrimaryKey = dt1.Columns.Cast<DataColumn>().Select(dt1col => dt1col).Where(dt1col => dt2.Columns.Contains(dt1col.ColumnName)).ToArray();
            DataColumn[] dt2PrimaryKey = dt2.Columns.Cast<DataColumn>().Select(dt2col => dt2col).Where(dt2col => dt1.Columns.Contains(dt2col.ColumnName)).ToArray();

            dt1.PrimaryKey = dt1PrimaryKey;
            dt2.PrimaryKey = dt2PrimaryKey;

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

                int rows = dt.Rows.Count;
                int cols = dt.Columns.Count;

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

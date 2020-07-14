using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

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
                throw new ArgumentNullException();
            }
            else if (filepaths.Length == 2)
            {
                Merge(filepaths[0], filepaths[1], output, culture);
            }
            else
            {
                LinkedList<string> tempFilepaths = new LinkedList<string>();

                using (FileStream tempStream = File.OpenWrite(Path.GetTempFileName()))
                {
                    Merge(filepaths[0], filepaths[1], tempStream, culture);
                    tempFilepaths.AddLast(tempStream.Name);
                }

                foreach (string filepath in filepaths.Skip(2).SkipLast(1))
                {
                    using (FileStream tempStream = File.OpenWrite(Path.GetTempFileName()))
                    {
                        Merge(tempFilepaths.Last.Value, filepath, tempStream, culture);
                        tempFilepaths.AddLast(tempStream.Name);
                        if (File.Exists(tempFilepaths.Last.Previous.Value))
                            File.Delete(tempFilepaths.Last.Previous.Value);
                    }
                }

                Merge(tempFilepaths.Last.Value, filepaths.Last(), output, culture);
                if (File.Exists(tempFilepaths.Last.Value))
                    File.Delete(tempFilepaths.Last.Value);
            }
        }

        public static void Merge(string filepath1, string filepath2, Stream output, CultureInfo culture)
        {
            using (var writer = new StreamWriter(output))
            using (var csvWriter = new CsvWriter(writer, culture))
            {
                csvWriter.Configuration.TrimOptions = TrimOptions.Trim;

                using (var reader1 = new StreamReader(filepath1))
                using (var csv1 = new CsvReader(reader1, culture))
                using (var reader2 = new StreamReader(filepath2))
                using (var csv2 = new CsvReader(reader2, culture))
                {
                    csv1.Configuration.TrimOptions = TrimOptions.Trim;
                    csv2.Configuration.TrimOptions = TrimOptions.Trim;

                    using (var dr1 = new CsvDataReader(csv1))
                    using (var dr2 = new CsvDataReader(csv2))
                    {
                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();
                        dt1.Load(dr1);
                        dt2.Load(dr2);

                        DataColumn[] dt1PrimaryKey = dt1.Columns.Cast<DataColumn>().Select(dt1col => dt1col).Where(dt1col => dt2.Columns.Contains(dt1col.ColumnName)).ToArray();
                        DataColumn[] dt2PrimaryKey = dt2.Columns.Cast<DataColumn>().Select(dt2col => dt2col).Where(dt2col => dt1.Columns.Contains(dt2col.ColumnName)).ToArray();
                        dt1.PrimaryKey = dt1PrimaryKey;
                        dt2.PrimaryKey = dt2PrimaryKey;
                        dt1.Merge(dt2);

                        int rows = dt1.Rows.Count;
                        int cols = dt1.Columns.Count;

                        for (int i = 0; i < cols; i++)
                        {
                            csvWriter.WriteField(dt1.Columns[i]);
                        }
                        csvWriter.NextRecord();

                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                csvWriter.WriteField(dt1.Rows[i][dt1.Columns[j]]);
                            }
                            csvWriter.NextRecord();
                        }
                    }
                }
            }
        }
    }
}

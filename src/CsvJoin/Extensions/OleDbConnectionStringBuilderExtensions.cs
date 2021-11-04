using System.Data.OleDb;

namespace CsvJoin.Extensions
{
    public static class OleDbConnectionStringBuilderExtensions
    {
        public static void AddProvider(
            this OleDbConnectionStringBuilder builder,
            string provider) => 
                builder.Add("Provider", provider);

        public static void AddDataSource(
            this OleDbConnectionStringBuilder builder,
            string dataSource) =>
                builder.Add("Data Source", dataSource);

        public static void AddOleDbServices(
            this OleDbConnectionStringBuilder builder,
            string oleDbServices) =>
                builder.Add("OLE DB Services", oleDbServices);

        public static void AddExtendedProperties(
            this OleDbConnectionStringBuilder builder,
            string extendedProperties) =>
                builder.Add("Extended Properties", extendedProperties);
    }
}

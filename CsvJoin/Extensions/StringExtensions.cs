namespace CsvJoin.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(
            this string value,
            char indentChar,
            int size)
        {
            string indentString = new string(indentChar, size);

            return string.Concat(indentString, value);
        }
    }
}

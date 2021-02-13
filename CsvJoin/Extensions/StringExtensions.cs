namespace CsvJoin.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(
            this string value,
            char indentChar,
            int count)
        {
            string indentString = new string(indentChar, count);

            return string.Concat(indentString, value);
        }
    }
}

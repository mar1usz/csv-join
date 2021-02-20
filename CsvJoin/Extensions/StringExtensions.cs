namespace CsvJoin.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(
            this string value,
            char indentChar,
            int indentation)
        {
            string indentString = new string(indentChar, indentation);

            return string.Concat(indentString, value);
        }
    }
}

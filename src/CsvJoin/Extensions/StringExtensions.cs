namespace CsvJoin.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(
            this string value,
            int indentation,
            char indentChar = ' ')
        {
            string indentString = new string(indentChar, indentation);
            return indentString + value;
        }
    }
}

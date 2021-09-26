namespace Vegas.NetCore.Common.Extensions
{
    public static class StringExtensions
    {
        public static string CommaToDot(this string str) => str.Replace(",", ".");

        public static bool NotContainsWhitespace(this string str) => !ContainsWhitespace(str);

        public static bool ContainsWhitespace(this string str) => str.Contains(" ");
    }
}

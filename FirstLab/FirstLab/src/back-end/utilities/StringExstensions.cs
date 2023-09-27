using System.Text;

namespace FirstLab.src.back_end.utilities
{
    public static class StringExtensions
    {
        public static string ExtractCapLetters(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                result.Append(char.ToUpper(c));
            }

            return result.ToString();
        }

        public static bool ContainsSymbols(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
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
    }
}

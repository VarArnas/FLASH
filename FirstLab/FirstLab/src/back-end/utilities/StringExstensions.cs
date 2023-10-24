using FirstLab.src.back_end.factories.factoryImplementations;
using FirstLab.src.back_end.factories.factoryInterfaces;
using System.Linq;

namespace FirstLab.src.back_end.utilities;

public static class StringExtensions
{

    public static IFactoryContainer factoryContainer;
    public static string ExtractCapLetters(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return new string(input.Where(char.IsUpper).ToArray());
    }

    public static string Capitalize(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return new string(input.Select(char.ToUpper).ToArray());
    }

    public static bool ContainsSymbols(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        return input.Any(c => char.IsSymbol(c));
    }
}
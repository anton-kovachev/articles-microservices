namespace Blocks.Core.Extensions;

public static class StringExtensions
{
    public static string FormatWith(this string @this, params object[] additionalArgs) => string.Format(@this, additionalArgs);
    public static string FormatWith(this string @this, object arg) => string.Format(@this, arg);
    public static int? ToInteger(this string input)
    {
        if (int.TryParse(input, out int i))
            return i;

        return null;
    }
}

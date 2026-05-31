
namespace Blocks.Core;

public static class DateTimeExtensions
{
    public static long ToUnixEpochDate(this DateTime dateTime)
        => (long)Math.Round(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
}

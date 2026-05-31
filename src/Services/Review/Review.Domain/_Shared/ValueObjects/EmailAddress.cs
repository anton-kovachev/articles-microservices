using Blocks.Core;
using Blocks.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace Review.Domain._Shared.ValueObjects;

public class EmailAddress : StringValueObject
{
    internal EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        Guard.ThrowIfFalse(!IsValidEmail(value), "Invalid email format.");

        return new EmailAddress(value);
    }

    private static bool IsValidEmail(string email)
    {
        const string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase);
    }


    public static implicit operator EmailAddress(string value)
        => Create(value);

    public static implicit operator string(EmailAddress @object) => @object.Value;

    public static bool operator ==(EmailAddress a, string b)
    {
        if (ReferenceEquals(a, null) && b == null)
            return true;

        if (ReferenceEquals(a, null) || b == null)
            return false;

        return string.Equals(a.Value, b, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool operator !=(EmailAddress a, string b) => !(a == b);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}

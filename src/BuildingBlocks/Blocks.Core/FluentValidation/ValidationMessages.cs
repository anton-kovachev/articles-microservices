namespace Blocks.Core.FluentValidation;
public static class ValidationMessages
{
    public const string InvalidId = "The {0} should be greater than zero.";
    public const string MaxLengthExceeded = "{0} must not exceed {1} characters.";
    public const string NullOrEmptyValue = "{0} is required.";
}

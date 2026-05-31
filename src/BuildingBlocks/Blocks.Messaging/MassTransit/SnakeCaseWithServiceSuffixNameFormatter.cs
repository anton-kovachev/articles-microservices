using MassTransit;

namespace Blocks.Messaging.MassTransit;

public class SnakeCaseWithServiceSuffixNameFormatter : SnakeCaseEndpointNameFormatter
{
    private readonly string _serviceName;

    public SnakeCaseWithServiceSuffixNameFormatter(string serviceName)
    {
        if(string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("Service name cannot be null or whitespace.", nameof(serviceName));

        _serviceName = serviceName.Trim().ToLowerInvariant();
    }

    public override string SanitizeName(string name)
    {
        name = name
            .Replace("EventHandler", "")
            .Replace("Handler", "");

        name = base.SanitizeName(name);
        return $"{name}.{_serviceName}";
    }
}

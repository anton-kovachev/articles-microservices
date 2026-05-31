using System.ComponentModel.DataAnnotations;

namespace Blocks.AspNetCore.Grpc;

public class GrpcServiceOptions
{
    public GrpcRetrySettings Retry { get; init; } = null!;
    public Dictionary<string, GrpcServiceSetting> Services { get; init; } = null!;
}

public class GrpcServiceSetting
{
    public string Url { get; init; } = null!;
    public bool EnableRetry { get; init; }
}

public class GrpcRetrySettings
{
    [Range(1, 10)]
    public int Count { get; init; }
    [Range(10, 10000)]
    public int InitialDelaysMs { get; init; }
}

namespace Articles.Security;

public record JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }
    public int ValidForMinutes { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public TimeSpan ValidFor => TimeSpan.FromMinutes(ValidForMinutes);
    public DateTime Expiration => IssuedAt.Add(ValidFor);
}

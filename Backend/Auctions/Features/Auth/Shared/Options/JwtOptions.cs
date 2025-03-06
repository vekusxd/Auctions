namespace Auctions.Features.Auth.Shared.Options;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";

    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpirationTimeInMinutes { get; init; }
    public int RefreshTokenExpirationTimeInDays { get; init; }
}
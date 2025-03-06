namespace Auctions.Features.Auth.Shared.Responses;

public record AuthResponse(string AccessToken, string RefreshToken, int ExpiresIn);
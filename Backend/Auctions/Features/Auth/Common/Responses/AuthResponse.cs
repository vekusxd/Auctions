namespace Auctions.Features.Auth.Common.Responses;

public record AuthResponse(string AccessToken, string RefreshToken, int ExpiresIn);
using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auth.Common.Options;
using Auctions.Features.Auth.Common.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auctions.Features.Auth.Refresh;

public class Refresh : EndpointWithoutRequest<Results<Ok, UnauthorizedHttpResult>>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthTokenProcessor _authTokenProcessor;
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtOptions> _jwtOptions;

    public Refresh(
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        AuthTokenProcessor authTokenProcessor,
        UserManager<User> userManager,
        IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _authTokenProcessor = authTokenProcessor;
        _userManager = userManager;
        _jwtOptions = jwtOptions;
    }

    public override async Task<Results<Ok, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
    {
        var refreshToken = _httpContextAccessor.HttpContext!.Request.Cookies[AuthTokenProcessor.RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return TypedResults.Unauthorized();
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken: ct);

        if (user == null)
        {
            return TypedResults.Unauthorized();
        }

        if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            return TypedResults.Unauthorized();
        }

        var jwtResult = _authTokenProcessor.GenerateJwtToken(user);
        var newRefreshToken = _authTokenProcessor.GenerateRefreshToken();

        var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpirationTimeInDays);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiresAt = refreshTokenExpirationDate;

        await _userManager.UpdateAsync(user);

        _authTokenProcessor.WriteAuthTokenAsHttpCookieOnly(AuthTokenProcessor.AccessTokenCookieName, jwtResult.JwtToken,
            jwtResult.ExpiresAt);
        _authTokenProcessor.WriteAuthTokenAsHttpCookieOnly(AuthTokenProcessor.RefreshTokenCookieName, newRefreshToken,
            refreshTokenExpirationDate);

        return TypedResults.Ok();
    }

    public override void Configure()
    {
        Post("/account/refresh");
        AllowAnonymous();
    }
}
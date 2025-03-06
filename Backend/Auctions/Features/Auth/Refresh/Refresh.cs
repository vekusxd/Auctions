using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auth.Shared.Options;
using Auctions.Features.Auth.Shared.Responses;
using Auctions.Features.Auth.Shared.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auctions.Features.Auth.Refresh;

public record RefreshRequest(string RefreshToken);

public class Refresh : Endpoint<RefreshRequest, Results<Ok<AuthResponse>, UnauthorizedHttpResult>>
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

    public override async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult>> ExecuteAsync(RefreshRequest request,
        CancellationToken ct)
    {
        var refreshToken = request.RefreshToken;

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
        
        return TypedResults.Ok(new AuthResponse(jwtResult.JwtToken, newRefreshToken, jwtResult.ExpirationTime));
    }

    public override void Configure()
    {
        Post("/account/refresh");
        AllowAnonymous();
    }
}
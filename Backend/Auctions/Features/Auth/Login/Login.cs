using Auctions.Database.Entities;
using Auctions.Features.Auth.Common.Options;
using Auctions.Features.Auth.Common.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auctions.Features.Auth.Login;


//TODO добавить валидацию
public sealed record LoginRequest(string Email, string Password);

public class Login : Endpoint<LoginRequest, Results<NotFound, UnauthorizedHttpResult, Ok>>
{
    private readonly UserManager<User> _userManager;
    private readonly AuthTokenProcessor _authTokenProcessor;
    private readonly IOptions<JwtOptions> _jwtOptions;

    public Login(UserManager<User> userManager, AuthTokenProcessor authTokenProcessor, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _authTokenProcessor = authTokenProcessor;
        _jwtOptions = jwtOptions;
    }
    
    public override void Configure()
    {
       Post("/account/login");
       AllowAnonymous();
    }

    public override async Task<Results<NotFound, UnauthorizedHttpResult, Ok>> ExecuteAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return TypedResults.NotFound();
        if (!await _userManager.CheckPasswordAsync(user, request.Password)) return TypedResults.Unauthorized();

        var jwtResult = _authTokenProcessor.GenerateJwtToken(user);
        var refreshToken = _authTokenProcessor.GenerateRefreshToken();

        var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpirationTimeInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = refreshTokenExpirationDate;

        await _userManager.UpdateAsync(user);

        _authTokenProcessor.WriteAuthTokenAsHttpCookieOnly(AuthTokenProcessor.AccessTokenCookieName, jwtResult.JwtToken, jwtResult.ExpiresAt);
        _authTokenProcessor.WriteAuthTokenAsHttpCookieOnly(AuthTokenProcessor.RefreshTokenCookieName, refreshToken, refreshTokenExpirationDate);

        return TypedResults.Ok();
    }
}
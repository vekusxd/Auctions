using Auctions.Database.Entities;
using Auctions.Features.Auth.Shared.Options;
using Auctions.Features.Auth.Shared.Responses;
using Auctions.Features.Auth.Shared.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auctions.Features.Auth.Login;


public sealed record LoginRequest(string Email, string Password);

public class Login : Endpoint<LoginRequest, Results<NotFound, UnauthorizedHttpResult, Ok, Ok<AuthResponse>>>
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

    public override async Task<Results<NotFound, UnauthorizedHttpResult, Ok, Ok<AuthResponse>>> ExecuteAsync(LoginRequest request, CancellationToken ct)
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

        return TypedResults.Ok(new AuthResponse(jwtResult.JwtToken, refreshToken, jwtResult.ExpirationTime));
    }
}
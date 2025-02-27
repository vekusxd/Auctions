using Auctions.Database.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Auctions.Features.Auth.Register;

//TODO добавить валидацию
public sealed record RegisterRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class Register : Endpoint<RegisterRequest, Results<Conflict<string>, Ok, BadRequest<string>>>
{
    private readonly UserManager<User> _userManager;

    public Register(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public override async Task<Results<Conflict<string>, Ok, BadRequest<string>>> ExecuteAsync(RegisterRequest request,
        CancellationToken ct)
    {
        var userExists = await _userManager.FindByEmailAsync(request.Email) != null;
        if (userExists) return TypedResults.Conflict($"User with email {request.Email} already exists.");

        var user = Database.Entities.User.Create(request.Email, request.FirstName, request.LastName);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest(
                $"Registration failed with following errors: {string.Join(Environment.NewLine, result.Errors.Select(e => e.Description))}");
        }

        return TypedResults.Ok();
    }

    public override void Configure()
    {
        Post("account/register");
        AllowAnonymous();
    }
}
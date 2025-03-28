﻿using Microsoft.AspNetCore.Identity;

namespace Auctions.Database.Entities;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public ICollection<Lot> Lots { get; set; } = [];
    public ICollection<Bid> Bids { get; set; } = [];

    public static User Create(string email, string firstName, string lastName)
    {
        return new User
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}"; 
    }
}
namespace Auctions.Features.ImageModeration.SignEngine;

public class SignEngineOptions
{
    public static readonly string SectionName = "SignEngineOptions";
    public required string ApiUser { get; init; }
    public required string ApiSecret { get; init; }
    public required string Models { get; set; }
}
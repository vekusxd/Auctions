namespace Auctions.Features.MinioStorage.Shared.Options;

public class MinioOptions
{
    public const string SectionName = "MinioOptions";
    public required string Endpoint { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
}
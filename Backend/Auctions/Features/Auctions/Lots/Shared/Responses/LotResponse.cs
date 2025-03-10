namespace Auctions.Features.Auctions.Lots.Shared.Responses;

public class LotResponse
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string LotCategory { get; init; }
    public decimal StartPrice { get; init; }
    public decimal PriceStep { get; init; }
    public decimal CurrentPrice { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime EndDate { get; init; }
    public string ImgUrl { get; init; }
    public int NumberOfBids { get; init; }
}
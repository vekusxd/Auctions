namespace Auctions.Database.Entities;

public class Bid : BaseEntity
{
    public required Lot Lot { get; set; }
    public Guid LotId { get; set; }
    public required User Bidder { get; set; }
    public Guid BidderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public override string ToString()
    {
        return $"Amount: {Amount}, Date: {CreationDate}";
    }
}
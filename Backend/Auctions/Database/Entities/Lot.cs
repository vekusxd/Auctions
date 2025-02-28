namespace Auctions.Database.Entities;

public class Lot : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public LotCategory LotCategory { get; set; } = null!;
    public Guid LotCategoryId { get; set; }
    public decimal StartPrice { get; set; }
    public decimal PriceStep { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public User Seller { get; set; } = null!;
    public Guid SellerId { get; set; }
    public string ImgUrl { get; set; } = string.Empty;
    public LotStatus Status { get; set; } = LotStatus.OnModeration;
    public ICollection<Bid> Bids { get; set; } = [];
    
    public override string ToString()
    {
        return $"Title: {Title}, CurrentBid: {CurrentPrice}, Status: {Status.ToString()}";
    }
}
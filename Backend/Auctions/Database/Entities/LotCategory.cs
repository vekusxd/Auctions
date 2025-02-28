namespace Auctions.Database.Entities;

public class LotCategory : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Lot> Lots { get; set; } = [];
    public override string ToString()
    {
        return $"Title: {Title}";
    }
}
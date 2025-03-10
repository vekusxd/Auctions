using Auctions.Database.Entities;
using Auctions.Features.Auctions.Lots.Shared.Responses;
using FastEndpoints;

namespace Auctions.Features.Auctions.Lots.Shared.Mappers;

public class LotResponseMapper : ResponseMapper<List<LotResponse>, List<Lot>>
{
    public override List<LotResponse> FromEntity(List<Lot> lots)
    {
        return lots.Select(lot => new LotResponse
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description ?? string.Empty,
            LotCategory = lot.LotCategory.Title,
            StartPrice = lot.StartPrice,
            PriceStep = lot.PriceStep,
            CurrentPrice = lot.CurrentPrice,
            CreationDate = lot.CreationDate,
            EndDate = lot.EndDate,
            ImgUrl = lot.ImgUrl,
            NumberOfBids = lot.Bids.Count
        }).ToList();
    }
}
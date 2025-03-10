using Auctions.Database.Entities;
using Auctions.Features.Auctions.LotCategories.GetAllLotCategories;
using FastEndpoints;

namespace Auctions.Features.Auctions.Lots.GetLotDetails;

public class LotDetailsMapper : ResponseMapper<GetLotDetailsResponse, Lot>
{
    public override GetLotDetailsResponse FromEntity(Lot lot)
    {
        return new GetLotDetailsResponse(
            Id: lot.Id,
            Title: lot.Title,
            Description: lot.Description ?? string.Empty,
            LotCategory: new LotCategoryResponse(lot.LotCategory.Id, lot.LotCategory.Title,
                lot.LotCategory.Description ?? string.Empty),
            StartPrice: lot.StartPrice,
            PriceStep: lot.PriceStep,
            CurrentPrice: lot.CurrentPrice,
            CreationDate: lot.CreationDate,
            EndDate: lot.EndDate,
            Seller: new UserResponse(lot.SellerId, lot.Seller.Email!, lot.Seller.ToString()),
            ImgUrl: lot.ImgUrl,
            Bids: lot.Bids.Select(b => new BidResponse(b.Id, b.Amount, b.CreationDate,
                new UserResponse(b.BidderId, b.Bidder.Email!, b.Bidder.ToString()))).ToList()
        );
    }
}
using Auctions.Database.Entities;
using FastEndpoints;

namespace Auctions.Features.Auctions.Lots.CreateLot;

public class LotMapper : Mapper<CreateLotRequest, CreateLotResponse, Lot>
{
    public override CreateLotResponse FromEntity(Lot lot)
    {
        return new CreateLotResponse(
            lot.Id,
            lot.Title, 
            lot.Description,
            lot.LotCategoryId,
            lot.StartPrice,
            lot.PriceStep, 
            lot.EndDate);
    }

    public override Lot ToEntity(CreateLotRequest lotRequest)
    {
        return new Lot
        {
            Title = lotRequest.Title,
            Description = lotRequest.Description,
            LotCategoryId = lotRequest.LotCategoryId,
            StartPrice = lotRequest.StartPrice,
            PriceStep = lotRequest.PriceStep,
            EndDate = lotRequest.EndDate,
            CreationDate = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CurrentPrice = lotRequest.StartPrice,
            Status = LotStatus.OnModeration,
            ImgUrl = lotRequest.ImgUrl
        };
    }
}
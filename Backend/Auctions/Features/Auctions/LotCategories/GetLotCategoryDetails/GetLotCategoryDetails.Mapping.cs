using Auctions.Database.Entities;
using FastEndpoints;

namespace Auctions.Features.Auctions.LotCategories.GetLotCategoryDetails;

public class GetLotCategoryDetailsMapper : ResponseMapper<GetLotCategoryDetailsResponse, LotCategory>
{
    public override GetLotCategoryDetailsResponse FromEntity(LotCategory lotCategory)
    {
        return new GetLotCategoryDetailsResponse(
            lotCategory.Id,
            lotCategory.Title,
            lotCategory.Description ?? string.Empty,
            lotCategory.Lots.Select(lc => new Lot(lc.Id, lc.Title, lc.Description ?? string.Empty))
        );
    }
}
using Auctions.Database.Entities;
using FastEndpoints;

namespace Auctions.Features.Auctions.LotCategories.GetAllLotCategories;

public class LotCategoryMapper : ResponseMapper<LotCategoryResponse, LotCategory>
{
    public override LotCategoryResponse FromEntity(LotCategory e)
    {
        return new LotCategoryResponse(e.Id, e.Title, e.Description ?? string.Empty);
    }
}
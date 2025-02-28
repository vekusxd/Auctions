using Auctions.Database.Entities;
using FastEndpoints;

namespace Auctions.Features.Auctions.LotCategories.CreateLotCategory;

public class CreateLogCategoryMapper : Mapper<CreateLotCategoryRequest, CreateLotCategoryResponse, LotCategory>
{
    public override CreateLotCategoryResponse FromEntity(LotCategory lotCategory)
    {
        return new CreateLotCategoryResponse(lotCategory.Id, lotCategory.Title,
            lotCategory.Description ?? string.Empty);
    }

    public override LotCategory ToEntity(CreateLotCategoryRequest request)
    {
        return new LotCategory
        {
            Title = request.Title,
            Description = request.Description
        };
    }
}
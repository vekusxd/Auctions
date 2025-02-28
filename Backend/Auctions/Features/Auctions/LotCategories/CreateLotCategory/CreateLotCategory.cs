using Auctions.Database;
using Auctions.Features.Auctions.LotCategories.GetLotCategoryDetails;
using FastEndpoints;

namespace Auctions.Features.Auctions.LotCategories.CreateLotCategory;

public record CreateLotCategoryRequest(string Title, string? Description);

public record CreateLotCategoryResponse(Guid Id, string Title, string Description);

public class CreateLotCategory : Endpoint<CreateLotCategoryRequest, CreateLotCategoryResponse, CreateLogCategoryMapper>
{
    private readonly AppDbContext _dbContext;

    public CreateLotCategory(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/lotCategories");
    }

    public override async Task HandleAsync(CreateLotCategoryRequest request, CancellationToken ct)
    {
        var entity = Map.ToEntity(request);
        _dbContext.LotCategories.Add(entity);
        await _dbContext.SaveChangesAsync(ct);

        await SendCreatedAtAsync<GetLotCategoryDetails.GetLotCategoryDetails>(
            new GetLotCategoryDetailsRequest(entity.Id), Map.FromEntity(entity), cancellation: ct);
    }
}


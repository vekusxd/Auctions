using Auctions.Database;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.LotCategories.GetLotCategoryDetails;

public record GetLotCategoryDetailsRequest(Guid Id);

public record GetLotCategoryDetailsResponse(
    Guid Id,
    string Title,
    string Description,
    IEnumerable<Lot> Lots);

public record Lot(Guid Id, string Title, string Description);

public class GetLotCategoryDetails : Endpoint<GetLotCategoryDetailsRequest, Results<NotFound, Ok<GetLotCategoryDetailsResponse>>, GetLotCategoryDetailsMapper>
{
    private readonly AppDbContext _dbContext;

    public GetLotCategoryDetails(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/lotCategories/{id}");
    }

    public override async Task<Results<NotFound, Ok<GetLotCategoryDetailsResponse>>> ExecuteAsync(GetLotCategoryDetailsRequest req, CancellationToken ct)
    {
        var category = await _dbContext.LotCategories
            .Include(lc => lc.Lots)
            .FirstOrDefaultAsync(c => c.Id == req.Id, ct);
        if (category == null) return TypedResults.NotFound();

        return TypedResults.Ok(Map.FromEntity(category));
    }
}
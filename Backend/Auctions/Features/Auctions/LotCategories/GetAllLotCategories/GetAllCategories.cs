using Auctions.Database;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.LotCategories.GetAllLotCategories;

public record LotCategoryResponse(Guid Id, string Title, string Description);

public class GetAllCategories : EndpointWithoutRequest<IEnumerable<LotCategoryResponse>, LotCategoryMapper> 
{
    private readonly AppDbContext _dbContext;

    public GetAllCategories(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/lotCategories");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var categories = await _dbContext.LotCategories.ToListAsync(ct);
        Response = categories.Select(Map.FromEntity);
    }
}
using Auctions.Database;
using Auctions.Features.Shared.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.LotCategories.GetAllLotCategories;

public record LotCategoryResponse(
    Guid Id,
    string Title,
    string Description
);

public class GetAllCategories : Endpoint<PagingRequest, IEnumerable<LotCategoryResponse>, LotCategoryMapper>
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

    public override async Task HandleAsync(PagingRequest request, CancellationToken ct)
    {
        var categories = await _dbContext.LotCategories
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);
        Response = categories.Select(Map.FromEntity);
    }
}
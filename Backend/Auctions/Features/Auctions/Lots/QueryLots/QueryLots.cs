using System.Linq.Expressions;
using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auctions.Lots.Shared.Mappers;
using Auctions.Features.Auctions.Lots.Shared.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.Lots.QueryLots;

public class QueryLotsRequest
{
    [QueryParam] public string? Title { get; set; }
    [QueryParam] public string? CategoryId { get; set; }
    [QueryParam] public int? PageSize { get; init; }
    [QueryParam] public int? PageNumber { get; init; }
    [QueryParam] public SortDirection? SortDirection { get; init; }
}

public class QueryLots :
    Endpoint<QueryLotsRequest, IEnumerable<LotResponse>, LotResponseMapper>
{
    private readonly AppDbContext _dbContext;

    public QueryLots(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/lots");
    }

    public override async Task HandleAsync(QueryLotsRequest req, CancellationToken ct)
    {
        var lots = _dbContext.Lots
            .AsNoTracking()
            .Sort(req.SortDirection)
            .Filter(req.Title, req.CategoryId, req.PageSize ?? 20, req.PageNumber ?? 1)
            .Include(l => l.LotCategory)
            .Include(l => l.Bids);

        Response = Map.FromEntity(await lots.ToListAsync(ct));
    }
}

public static class LotExtension
{
    public static IQueryable<Lot> Filter(
        this IQueryable<Lot> lots,
        string? title,
        string? categoryId,
        int pageSize,
        int pageNumber)
    {
        if (!string.IsNullOrEmpty(title))
            lots = lots.Where(l => l.Title.ToLower().Contains(title.ToLower()));

        if (!string.IsNullOrEmpty(categoryId))
            lots = lots.Where(l => l.LotCategory.Id.ToString() == categoryId);

        return lots
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<Lot> Sort(this IQueryable<Lot> lots, SortDirection? sortDirection)
    {
        if (sortDirection == null)
            return lots;

        return sortDirection == SortDirection.Ascending
            ? lots.OrderBy(l => l.CreationDate)
            : lots.OrderByDescending(l => l.CreationDate);
    }
}

public enum SortDirection
{
    Ascending,
    Descending
}
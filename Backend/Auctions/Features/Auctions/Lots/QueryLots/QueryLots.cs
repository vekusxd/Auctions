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

public class QueryLotsResponse
{
    public required IEnumerable<LotResponse> Data { get; init; }
    public required int TotalItems { get; init; }
}

public class QueryLots :
    Endpoint<QueryLotsRequest, QueryLotsResponse, LotResponseMapper>
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

    public override async Task HandleAsync(QueryLotsRequest request, CancellationToken ct)
    {
        var lotsQuery = _dbContext.Lots
            .AsNoTracking()
            .Sort(request.SortDirection)
            .Filter(request.Title, request.CategoryId);

        var totalCount = await lotsQuery.CountAsync(ct);

        var items = await lotsQuery
            .Page(request.PageNumber ?? 1, request.PageSize ?? 10)
            .Include(l => l.LotCategory)
            .Include(l => l.Bids)
            .ToListAsync(ct);

        Response = new QueryLotsResponse { TotalItems = totalCount, Data = Map.FromEntity(items) };
    }
}

public static class LotExtension
{
    public static IQueryable<Lot> Filter(
        this IQueryable<Lot> lots,
        string? title,
        string? categoryId)
    {
        if (!string.IsNullOrEmpty(title))
            lots = lots.Where(l => l.Title.ToLower().Contains(title.ToLower()));

        if (!string.IsNullOrEmpty(categoryId))
            lots = lots.Where(l => l.LotCategory.Id.ToString() == categoryId);

        return lots;
    }

    public static IQueryable<Lot> Sort(this IQueryable<Lot> lots, SortDirection? sortDirection)
    {
        if (sortDirection == null)
            return lots;

        return sortDirection == SortDirection.Ascending
            ? lots.OrderBy(l => l.CreationDate)
            : lots.OrderByDescending(l => l.CreationDate);
    }

    public static IQueryable<Lot> Page(this IQueryable<Lot> lots, int pageNumber, int pageSize)
    {
        return lots
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}

public enum SortDirection
{
    Ascending,
    Descending
}
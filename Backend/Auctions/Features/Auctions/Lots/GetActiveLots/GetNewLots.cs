using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auctions.Lots.Shared.Mappers;
using Auctions.Features.Auctions.Lots.Shared.Responses;
using Auctions.Features.Shared.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.Lots.GetActiveLots;

public class GetNewLots : Endpoint<PagingRequest, List<LotResponse>, LotResponseMapper>
{
    private readonly AppDbContext _dbContext;

    public GetNewLots(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/lots/active");
    }

    public override async  Task HandleAsync(PagingRequest request, CancellationToken ct)
    {
        var lots = _dbContext.Lots
            .AsNoTracking()
            .OrderByDescending(l => l.CreationDate)
            .Include(l => l.LotCategory)
            .Include(l => l.Bids)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        Response = Map.FromEntity(await lots.ToListAsync(ct));
    }
}

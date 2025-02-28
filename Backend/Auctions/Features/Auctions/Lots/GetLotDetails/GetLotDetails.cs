using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auctions.LotCategories.GetAllLotCategories;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.Lots.GetLotDetails;

public record GetLotDetailsRequest(Guid Id);

public record GetLotDetailsResponse(
    Guid Id,
    string Title,
    string Description,
    LotCategoryResponse LotCategory,
    decimal StartPrice,
    decimal PriceStep,
    decimal CurrentPrice,
    DateTime CreationDate,
    DateTime EndDate,
    UserResponse Seller,
    ICollection<BidResponse> Bids);

public record UserResponse(Guid Id, string Email, string Name);

public record BidResponse(Guid Id, decimal Amount, DateTime CreationDate, UserResponse Bidder);

public class GetLotDetails : Endpoint<GetLotDetailsRequest, Results<Ok<GetLotDetailsResponse>, NotFound<string>>,
    LotDetailsMapper>
{
    private readonly AppDbContext _dbContext;

    public GetLotDetails(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/lots/{id}");
    }

    public override async Task<Results<Ok<GetLotDetailsResponse>, NotFound<string>>> ExecuteAsync(
        GetLotDetailsRequest request, CancellationToken ct)
    {
        var lot = await _dbContext.Lots
            .Include(l => l.Seller)
            .Include(l => l.Bids)
            .ThenInclude(b => b.Bidder)
            .Include(l => l.LotCategory)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken: ct);
        if (lot == null) return TypedResults.NotFound($"Lot with id {request.Id} not found");

        return TypedResults.Ok(Map.FromEntity(lot));
    }
}
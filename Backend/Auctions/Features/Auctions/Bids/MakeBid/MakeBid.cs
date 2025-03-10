using Auctions.Database;
using Auctions.Database.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.Bids.MakeBid;

public record MakeBidRequest(
    Guid LotId
);

public class MakeBid : Endpoint<MakeBidRequest, Results<Ok, NotFound, BadRequest, Conflict>>
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public MakeBid(AppDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/bid");
    }

    public override async Task<Results<Ok, NotFound, BadRequest, Conflict>> ExecuteAsync(MakeBidRequest request,
        CancellationToken ct)
    {
        var lot = await _dbContext.Lots
            .Include(l => l.Bids)
            .ThenInclude(b => b.Bidder)
            .FirstOrDefaultAsync(l => l.Id == request.LotId, ct);

        if (lot == null) return TypedResults.NotFound();

        var user = await _userManager.GetUserAsync(User);

        if (user!.Id == lot.SellerId) return TypedResults.Conflict();

        var lastBidder = lot.Bids.OrderBy(b => b.CreationDate).LastOrDefault();

        if (lastBidder != null && lastBidder.BidderId == user!.Id) return TypedResults.BadRequest();
        
        var bid = new Bid
            { Amount = lot.CurrentPrice + lot.PriceStep, Lot = lot, Bidder = user!, CreationDate = DateTime.UtcNow };
        _dbContext.Bids.Add(bid);
        lot.CurrentPrice = bid.Amount;
        await _dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }
}
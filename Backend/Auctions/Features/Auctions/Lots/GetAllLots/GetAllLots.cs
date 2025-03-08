using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Shared.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Features.Auctions.Lots.GetAllLots;


public class GetAllLotsRequest : PagingRequest
{
    public string? Search { get; set; }
}

public class Response
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string LotCategory { get; init; }
    public decimal StartPrice { get; init; }
    public decimal PriceStep { get; init; }
    public decimal CurrentPrice { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime EndDate { get; init; }
    public string ImgUrl { get; init; }
}

public class ResponseMapper : ResponseMapper<List<Response>, List<Lot>>
{
    public override List<Response> FromEntity(List<Lot> lots)
    {
        return lots.Select(lot => new Response
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description ?? string.Empty,
            LotCategory = lot.LotCategory.Title,
            StartPrice = lot.StartPrice,
            PriceStep = lot.PriceStep,
            CurrentPrice = lot.CurrentPrice,
            CreationDate = lot.CreationDate,
            EndDate = lot.EndDate,
            ImgUrl = lot.ImgUrl
        }).ToList();
    }
}

public class GetAllLots : Endpoint<GetAllLotsRequest, List<Response>, ResponseMapper>
{
    private readonly AppDbContext _dbContext;

    public GetAllLots(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/lots");
    }

    public override async Task HandleAsync(GetAllLotsRequest request, CancellationToken ct)
    {
        var lots =  _dbContext.Lots
            .AsNoTracking()
            .Include(l => l.LotCategory)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        if (request.Search != null)
        {
            lots = lots.Where(l => l.Title.ToLower().Contains(request.Search.ToLower()));
        }

        Response = Map.FromEntity(await lots.ToListAsync(ct));
    }
}
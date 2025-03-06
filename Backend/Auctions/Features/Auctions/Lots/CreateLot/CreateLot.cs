using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auctions.Database;
using Auctions.Database.Entities;
using Auctions.Features.Auctions.Lots.GetLotDetails;
using Auctions.Features.MinioStorage.MinioUpload;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace Auctions.Features.Auctions.Lots.CreateLot;

public record CreateLotRequest(
    string Title,
    string? Description,
    Guid LotCategoryId,
    decimal StartPrice,
    decimal PriceStep,
    DateTime EndDate,
    string ImgUrl
);
    
public record CreateLotResponse(
    Guid Id,
    string Title,
    string? Description,
    Guid LotCategoryId,
    decimal StartPrice,
    decimal PriceStep,
    DateTime EndDate
);

public class CreateLot : Endpoint<CreateLotRequest, CreateLotResponse, LotMapper>
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public CreateLot(AppDbContext dbContext, UserManager<User> userManager, MinioUploader minioUploader)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("/lots");
    }

    public override async Task HandleAsync(CreateLotRequest request, CancellationToken ct)
    {
        var lot = Map.ToEntity(request);
        var user = await _userManager.GetUserAsync(User);
        lot.Seller = user!;
        
         _dbContext.Lots.Add(lot);
         await _dbContext.SaveChangesAsync(ct);

         await SendCreatedAtAsync<GetLotDetails.GetLotDetails>(new GetLotDetailsRequest(lot.Id), Map.FromEntity(lot), cancellation: ct);
    }
}
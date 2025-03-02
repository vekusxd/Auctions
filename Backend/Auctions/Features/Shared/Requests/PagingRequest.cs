using FastEndpoints;

namespace Auctions.Features.Shared.Requests;

public record PagingRequest(
    [property: QueryParam] int PageSize = 5,
    [property: QueryParam] int PageNumber = 1);
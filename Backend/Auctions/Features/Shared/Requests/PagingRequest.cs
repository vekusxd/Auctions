using FastEndpoints;

namespace Auctions.Features.Shared.Requests;

public class PagingRequest()
{
    [QueryParam] public int PageSize { get; init; } = 5;
    [QueryParam] public int PageNumber { get; init; } = 1;
}
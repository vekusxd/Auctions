using FastEndpoints;
using FluentValidation;

namespace Auctions.Features.Auctions.Lots.QueryLots;

public class QueryLotsValidator : Validator<QueryLotsRequest>
{
    public QueryLotsValidator()
    {
        RuleFor(r => r.Title).MaximumLength(30).WithMessage("Max length 30 characters");
        RuleFor(r => r.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(r => r.PageSize)
            .LessThanOrEqualTo(30).WithMessage("Page size must be less than 30");
    }
}
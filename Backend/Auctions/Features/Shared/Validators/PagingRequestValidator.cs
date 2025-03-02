using Auctions.Features.Shared.Requests;
using FastEndpoints;
using FluentValidation;

namespace Auctions.Features.Shared.Validators;

public class PagingRequestValidator : Validator<PagingRequest>
{
    public PagingRequestValidator()
    {
        RuleFor(r => r.PageNumber)
            .NotNull().WithMessage("Page number is required")
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(r => r.PageSize)
            .NotNull().WithMessage("Page size is required")
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("Page size must be less than 50");
    }
}
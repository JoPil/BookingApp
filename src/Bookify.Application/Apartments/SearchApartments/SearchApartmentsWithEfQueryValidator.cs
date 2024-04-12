using FluentValidation;

namespace Bookify.Application.Apartments.SearchApartments;
internal sealed class SearchApartmentsWithEfQueryValidator
    : AbstractValidator<SearchApartmentsWithEfQuery>
{
    public SearchApartmentsWithEfQueryValidator()
    {
        RuleFor(m => m.StartDate)
           .NotEqual(default(DateOnly))
           .WithMessage("Start Date is Required");
           

        RuleFor(m => m.EndDate)
            .NotEqual(default(DateOnly))
            .WithMessage("End date is required")
            .GreaterThan(m => m.StartDate)
            .WithMessage("End date must be after Start date")
            ;

    }

}

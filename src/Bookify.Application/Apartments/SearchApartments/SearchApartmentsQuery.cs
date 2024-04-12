using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Apartments.SearchApartments;

public sealed record SearchApartmentsQuery(
    string? SearchTerm,
    string? KeySelector,
    string? SortDirection,
    DateOnly StartDate,
    DateOnly EndDate,
    int Page,
    int PageSize) : IQuery<IReadOnlyList<ApartmentResponse>>;

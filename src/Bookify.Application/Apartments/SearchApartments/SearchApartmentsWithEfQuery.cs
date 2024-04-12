using Bookify.Application.Abstractions.Common;
using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Apartments.SearchApartments;

public sealed record SearchApartmentsWithEfQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    DateOnly StartDate,
    DateOnly EndDate,
    int Page,
    int PageSize) : IQuery<PagedList<ApartmentResponse>>;

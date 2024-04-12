using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Application.Data;
using Bookify.Application.Abstractions.Common;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentsWithEfQueryHandler
    : IQueryHandler<SearchApartmentsWithEfQuery, PagedList<ApartmentResponse>>
{
    private readonly IApplicationDbContext _context;

    public SearchApartmentsWithEfQueryHandler( IApplicationDbContext context)
    {
		_context = context;
    }

    public async Task<Result<PagedList<ApartmentResponse>>> Handle(SearchApartmentsWithEfQuery request, CancellationToken cancellationToken)
    {

        if (request.StartDate > request.EndDate)
        {
			return Result.Failure<PagedList<ApartmentResponse>>(ApartmentErrors.NotFound);
		}

        IQueryable<Apartment> apartmentsQuery = _context.Apartments;

		if (!string.IsNullOrWhiteSpace(request.SearchTerm))
		{
			apartmentsQuery = apartmentsQuery
                .Where(a =>
				        ((string)a.Name).Contains(request.SearchTerm) ||
				        ((string)a.Description).Contains(request.SearchTerm))
				.Where(a =>
                        !_context.Bookings.Any(b =>
                        b.ApartmentId == a.Id &&
					    b.Duration.Start <= request.EndDate &&
					    b.Duration.End >= request.StartDate &&
					    (b.Status == BookingStatus.Reserved ||
                        b.Status == BookingStatus.Completed ||
                        b.Status == BookingStatus.Confirmed)
                        ));
        }


        apartmentsQuery = request.SortOrder?.ToLower() == "desc"
            ? apartmentsQuery.OrderByDescending(GetSortProperty(request))
            : (IQueryable<Apartment>)apartmentsQuery.OrderBy(GetSortProperty(request));

        IQueryable<ApartmentResponse> apartmentsResponsesQuery = apartmentsQuery
			.Select(a => new ApartmentResponse
			{
				Id = a.Id,
				Name = a.Name.Value,
				Description = a.Description.Value,
				Price = a.Price.Amount,
				Currency = a.Price.Currency.Code,
				Address = new AddressResponse
				{
					Country = a.Address.Country,
					State = a.Address.State,
					ZipCode = a.Address.ZipCode,
					City = a.Address.City,
					Street = a.Address.Street
				}

			});
        

        PagedList<ApartmentResponse> apartments = await PagedList<ApartmentResponse>.CreateAsync(
			apartmentsResponsesQuery,
            request.Page,
            request.PageSize);

        return apartments;
    }
	private static Expression<Func<Apartment, object>> GetSortProperty(SearchApartmentsWithEfQuery request) =>
		request.SortColumn?.ToLower() switch
		{
			"price" => apartment => apartment.Price.Amount,
			"name" => apartment => apartment.Name,
			_ => apartment => apartment.Id
		};
	
}

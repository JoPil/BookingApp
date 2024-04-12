using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public interface IApplicationDbContext
{
	DbSet<Apartment> Apartments { get; set; }

	DbSet<Booking> Bookings { get; set; }

	DbSet<Review> Reviews { get; set; }

	DbSet<User> Users { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Asp.Versioning;
using Bookify.Application.Abstractions.Common;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/apartments")]
public class ApartmentsController : ControllerBase
{
    private readonly ISender _sender;

    public ApartmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        DateOnly startDate,
        DateOnly endDate,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(searchTerm, sortColumn, sortOrder, startDate, endDate, page, pageSize);

        Result<IReadOnlyList<ApartmentResponse>> result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("WithEf")]
    public async Task<IActionResult> SearchApartmentsWithEf(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        DateOnly startDate,
        DateOnly endDate,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsWithEfQuery(searchTerm, sortColumn, sortOrder, startDate, endDate, page, pageSize);

        Result<PagedList<ApartmentResponse>> result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}

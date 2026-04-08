using Microsoft.AspNetCore.Mvc;
using PotteryService.Application.Features.Reports.Dtos;
using PotteryService.Application.Features.Reports.Requests;
using PotteryService.Application.Features.Reports.Services;

namespace PotteryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("sales-quantity")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SalesQuantityReportItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyCollection<SalesQuantityReportItemDto>>> GetSalesQuantity(
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to,
        CancellationToken cancellationToken)
    {
        var request = new SalesQuantityReportRequest(from, to);
        var report = await _reportService.GetSalesQuantityByDateRangeAsync(request, cancellationToken);

        return Ok(report);
    }

    [HttpGet("product-revenue")]
    [ProducesResponseType(typeof(ProductRevenueReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductRevenueReportDto>> GetProductRevenue(
        [FromQuery] string productName,
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to,
        CancellationToken cancellationToken)
    {
        var request = new ProductRevenueReportRequest(productName, from, to);
        var report = await _reportService.GetProductRevenueByDateRangeAsync(request, cancellationToken);

        return Ok(report);
    }
}

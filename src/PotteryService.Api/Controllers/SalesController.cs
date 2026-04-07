using Microsoft.AspNetCore.Mvc;
using PotteryService.Application.Features.Sales.Dtos;
using PotteryService.Application.Features.Sales.Requests;
using PotteryService.Application.Features.Sales.Services;

namespace PotteryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<SaleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<SaleDto>>> GetAll(CancellationToken cancellationToken)
    {
        var sales = await _saleService.GetAllAsync(cancellationToken);

        return Ok(sales);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SaleDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var sale = await _saleService.GetByIdAsync(id, cancellationToken);

        return Ok(sale);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SaleDto>> Create(
        [FromBody] CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var sale = await _saleService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
    }
}

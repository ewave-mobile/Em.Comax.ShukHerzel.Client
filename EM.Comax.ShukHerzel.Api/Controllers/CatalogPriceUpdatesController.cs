using System.Security.Claims;
using EM.Comax.Api.Dtos;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EM.Comax.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/branches/{branchId:long}/catalog-price-updates")]
public class CatalogPriceUpdatesController : ControllerBase
{
    private readonly ICatalogPriceApprovalService _catalogPriceApprovalService;

    public CatalogPriceUpdatesController(ICatalogPriceApprovalService catalogPriceApprovalService)
    {
        _catalogPriceApprovalService = catalogPriceApprovalService;
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<PendingCatalogPriceDto>>> GetPending(
        long branchId,
        [FromQuery] DateOnly? date = null)
    {
        DateTime? createdDate = date.HasValue
            ? date.Value.ToDateTime(TimeOnly.MinValue)
            : null;

        var pending = await _catalogPriceApprovalService.GetPendingByBranchAsync(branchId, createdDate);
        var result = pending
            .Select(row => new PendingCatalogPriceDto
            {
                Id = row.Id,
                Name = row.Name,
                Barcode = row.Barcode,
                OldPrice = row.OldPrice,
                NewPrice = row.NewPrice,
                CreatedDateTime = row.CreatedDateTime
            })
            .ToList();

        return Ok(result);
    }

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(long branchId, [FromBody] ApproveRequest request)
    {
        if (request?.Ids == null || request.Ids.Count == 0)
        {
            return BadRequest("No ids provided.");
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _catalogPriceApprovalService.ApproveAsync(request.Ids, userId);
        return Ok(new { approved = request.Ids.Count });
    }
}

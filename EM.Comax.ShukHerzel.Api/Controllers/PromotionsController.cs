using System.Security.Claims;
using EM.Comax.Api.Dtos;
using EM.Comax.ShukHerzel.Bl;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EM.Comax.Api.Controllers;

// 13/08/2026 - Y.E
[Authorize]
[ApiController]
[Route("api/branches/{branchId:long}/promotions")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionsService _promotionsService;

    public PromotionsController(IPromotionsService promotionsService)
    {
        _promotionsService = promotionsService;
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<PendingPromotionDto>>> GetPending(
        long branchId,
        [FromQuery] DateOnly? date = null)
    {
        DateTime? createdDate = date.HasValue
            ? date.Value.ToDateTime(TimeOnly.MinValue)
            : null;

        var pending = await _promotionsService.GetPendingApprovalByBranchAsync(branchId, createdDate);
        var result = pending
            .Select(p => new PendingPromotionDto
            {
                Id = p.Id,
                Nm = p.Nm,
                ItemKod = p.ItemKod,
                Kod = p.Kod,
                Total = p.Total,
                FromDate = p.FromDate,
                ToDate = p.ToDate,
                CreatedDateTime = p.CreatedDateTime
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

        try
        {
            await _promotionsService.ApproveOperTransferAsync(request.Ids, userId);
        }
        catch (CatalogPriceApprovalBlockedException ex)
        {
            return BadRequest(new { message = ex.Message, barcodes = ex.Barcodes });
        }

        return Ok(new { approved = request.Ids.Count });
    }
}

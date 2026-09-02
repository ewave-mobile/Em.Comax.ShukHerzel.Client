using System.Security.Claims;
using EM.Comax.Api.Dtos;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EM.Comax.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/branches/{branchId:long}/price-updates")]
public class PriceUpdatesController : ControllerBase
{
    private readonly IPriceUpdateService _priceUpdateService;

    public PriceUpdatesController(IPriceUpdateService priceUpdateService)
    {
        _priceUpdateService = priceUpdateService;
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<PendingPriceUpdateDto>>> GetPending(
        long branchId,
        [FromQuery] DateOnly? date = null)
    {
        DateTime? createdDate = date.HasValue
            ? date.Value.ToDateTime(TimeOnly.MinValue)
            : null;

        var pending = await _priceUpdateService.GetPendingApprovalByBranchAsync(branchId, createdDate);
        var result = pending
            .Select(pu => new PendingPriceUpdateDto
            {
                Id = pu.Id,
                Name = pu.Name,
                Barcode = pu.Barcode,
                NetPrice = pu.NetPrice,
                CreatedDateTime = pu.CreatedDateTime
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

        await _priceUpdateService.ApproveOperTransferAsync(request.Ids, userId);
        return Ok(new { approved = request.Ids.Count });
    }
}

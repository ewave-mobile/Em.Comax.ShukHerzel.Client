using EM.Comax.ShukHerzel.Api.Dtos;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Comax.ShukHerzel.Api.Controllers;

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
    public async Task<ActionResult<List<PendingPriceUpdateDto>>> GetPending(long branchId)
    {
        var pending = await _priceUpdateService.GetPendingApprovalByBranchAsync(branchId);
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

        await _priceUpdateService.ApproveOperTransferAsync(request.Ids);
        return Ok(new { approved = request.Ids.Count });
    }
}

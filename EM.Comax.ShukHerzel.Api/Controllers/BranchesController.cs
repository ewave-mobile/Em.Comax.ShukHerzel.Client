using EM.Comax.ShukHerzel.Api.Dtos;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Comax.ShukHerzel.Api.Controllers;

[ApiController]
[Route("api/branches")]
public class BranchesController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IBranchService _branchService;

    public BranchesController(ICompanyService companyService, IBranchService branchService)
    {
        _companyService = companyService;
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BranchDto>>> GetBranches()
    {
        var companies = await _companyService.GetAllCompanies();
        var approvalCompanyIds = companies
            .Where(c => c.IsOperTransferApprovalRequired)
            .Select(c => c.Id)
            .ToHashSet();

        if (approvalCompanyIds.Count == 0)
        {
            return Ok(new List<BranchDto>());
        }

        var branches = await _branchService.GetAllBranches();
        var result = branches
            .Where(b => b.CompanyId.HasValue && approvalCompanyIds.Contains(b.CompanyId.Value))
            .OrderBy(b => b.BranchName)
            .Select(b => new BranchDto
            {
                Id = b.Id,
                BranchName = b.BranchName ?? string.Empty,
                ComaxStoreId = b.ComaxStoreId,
                Description = b.Description
            })
            .ToList();

        return Ok(result);
    }
}

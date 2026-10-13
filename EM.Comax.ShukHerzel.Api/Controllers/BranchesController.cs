using EM.Comax.Api.Dtos;
using EM.Comax.ShukHerzel.Bl.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EM.Comax.Api.Controllers;

[Authorize]
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
        var approvalCompanies = companies
            .Where(c => c.IsOperTransferApprovalRequired || c.IsCatalogPriceApprovalRequired)
            .ToDictionary(c => c.Id);

        if (approvalCompanies.Count == 0)
        {
            return Ok(new List<BranchDto>());
        }

        var branches = await _branchService.GetAllBranches();
        var result = branches
            .Where(b => b.CompanyId.HasValue && approvalCompanies.ContainsKey(b.CompanyId.Value))
            .OrderBy(b => b.BranchName)
            .Select(b =>
            {
                var company = approvalCompanies[b.CompanyId!.Value];
                return new BranchDto
                {
                    Id = b.Id,
                    BranchName = b.BranchName ?? string.Empty,
                    ComaxStoreId = b.ComaxStoreId,
                    Description = b.Description,
                    IsOperTransferApprovalRequired = company.IsOperTransferApprovalRequired,
                    IsCatalogPriceApprovalRequired = company.IsCatalogPriceApprovalRequired
                };
            })
            .ToList();

        return Ok(result);
    }
}

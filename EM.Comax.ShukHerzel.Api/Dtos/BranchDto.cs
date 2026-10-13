namespace EM.Comax.Api.Dtos;

public class BranchDto
{
    public long Id { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public long? ComaxStoreId { get; set; }
    public string? Description { get; set; }
    public bool IsOperTransferApprovalRequired { get; set; }
    public bool IsCatalogPriceApprovalRequired { get; set; }
}

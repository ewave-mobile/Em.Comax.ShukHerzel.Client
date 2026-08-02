namespace EM.Comax.ShukHerzel.Api.Dtos;

public class BranchDto
{
    public long Id { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public long? ComaxStoreId { get; set; }
    public string? Description { get; set; }
}

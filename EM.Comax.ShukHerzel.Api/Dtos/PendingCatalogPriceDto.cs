namespace EM.Comax.Api.Dtos;

public class PendingCatalogPriceDto
{
    public long Id { get; set; }
    public string? Barcode { get; set; }
    public string? Name { get; set; }
    public decimal? OldPrice { get; set; }
    public string? NewPrice { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

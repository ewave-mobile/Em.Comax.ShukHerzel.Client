namespace EM.Comax.ShukHerzel.Api.Dtos;

public class PendingPriceUpdateDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Barcode { get; set; }
    public string? NetPrice { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

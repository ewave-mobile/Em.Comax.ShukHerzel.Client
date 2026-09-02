namespace EM.Comax.Api.Dtos;

public class PendingPromotionDto
{
    public long Id { get; set; }
    public string? Nm { get; set; }
    public string? ItemKod { get; set; }
    public string? Kod { get; set; }
    public string? Total { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

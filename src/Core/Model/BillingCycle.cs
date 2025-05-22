namespace Core.Model;

public record BillingCycle
{
    public EFrequencyUnit FrequencyUnit { get; set; }
    public ETenureType TenureType { get; set; }
    public int Sequence { get; set; }
    public int TotalCycles { get; set; }
    public Decimal Price { get; set; }
    public required string Currency { get; set; }
}
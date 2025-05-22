namespace Core.Model;

public record BillingEvent
{
    public Guid CorrelationId { get; set; }
    public decimal Price { get; set; }
    public DateTime TimeStamp { get; set; }
}
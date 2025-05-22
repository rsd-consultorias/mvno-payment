namespace Core.Model;

public record CheckoutProcessResponse
{
    public Subscription? Subscription { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
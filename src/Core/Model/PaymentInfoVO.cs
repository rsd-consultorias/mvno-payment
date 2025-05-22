namespace Core.Model;

public record PaymentInfoVO
{
    public required string PaymentPlatform { get; set; }
    public required string PlatformPayerId { get; set; }
    public required string PlatformTransactionId { get; set; }
    public required IPaymentMethod PaymentMethod { get; set; }
    public bool Approved { get; set; }
    public string? FailureReason { get; set; }
}
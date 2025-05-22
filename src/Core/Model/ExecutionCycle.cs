namespace Core.Model;

public record ExecutionCycle : BillingCycle
{
    public int CyclesCompleted { get; set; }
    public int CyclesRemaining { get; set; }
    public DateTime NextBillingTime { get; set; }
    public DateTime PaymentTime { get; set; }
    public DateTime LastPaymentTime { get; set; }
}
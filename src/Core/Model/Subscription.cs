namespace Core.Model;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public ESubscriptionState State { get; set; }
    public required CustomerInfo Subscriber { get; set; }
    public required ICollection<ExecutionCycle> ExecutionCycles { get; set; }
    public required string StatusChangeNote { get; set; }
    public DateTime StatusUpdateTime { get; set; }
    public DateTime StartTime { get; set; }
    public  PaymentInfoVO? PaymentInfo { get; set; }
}
using Core.Interface;
using Core.Model;

namespace Core.ApplicationService;

public class BillingService
{
    private ISubscriptionRepository subscriptionRepository;
    private IPlanRepository planRepository;

    public BillingService(ISubscriptionRepository subscriptionRepository, IPlanRepository planRepository)
    {
        this.subscriptionRepository = subscriptionRepository;
        this.planRepository = planRepository;
    }

    public void ProcessBillingEvent(BillingEvent billingEvent)
    {
        var subscription = subscriptionRepository.FindById(billingEvent.CorrelationId);
        var plan = planRepository.FindById(subscription.PlanId);
        var currentSequence = subscription.ExecutionCycles.Count;

        BillingCycle currentBillingCycle;
        Dictionary<int, int> cycleRange = [];
        int maxRange = 0;

        // Assemble a vector with range of each cycle
        foreach (var cycle in plan.BillingCycles)
        {
            maxRange += cycle.TotalCycles;
            cycleRange.Add(cycle.Sequence, maxRange - 1);
        }

        // Find the current cycle according to the cycles in plan configuration and the completed cycles
        currentBillingCycle = plan.BillingCycles.First(x => x.Sequence.Equals(cycleRange.First(x => x.Value >= currentSequence).Key));
        int totalCycles = plan.BillingCycles.Sum(x => x.TotalCycles);
        
        // Add a subscription cycle with the cycle and sequence
        subscription.ExecutionCycles.Add(new ExecutionCycle
        {
            Currency = currentBillingCycle.Currency,
            Price = billingEvent.Price,
            CyclesCompleted = currentSequence + 1,
            CyclesRemaining = totalCycles - currentSequence - 1,
            FrequencyUnit = currentBillingCycle.FrequencyUnit,
            LastPaymentTime = billingEvent.TimeStamp,
            NextBillingTime = billingEvent.TimeStamp.AddMonths(1),
            PaymentTime = billingEvent.TimeStamp,
            Sequence = currentSequence,
            TenureType = currentBillingCycle.TenureType,
            TotalCycles = totalCycles
        });

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.StatusChangeNote = $"Paid {currentSequence + 1} of {totalCycles}";

        subscriptionRepository.Save(subscription);
    }
}
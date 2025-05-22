namespace Core.Model;

public class Plan
{
    public Guid Id { get; set; }
    public string SKU { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
    public ICollection<BillingCycle> BillingCycles { get; set; }
    public ICollection<PlanDetailVO> PlanDetails { get; set; }

    public Plan(string sku, string name, string description, ICollection<BillingCycle> billingCycles, ICollection<PlanDetailVO> planDetails)
    {
        SKU = sku;
        Name = name;
        Description = description;
        BillingCycles = billingCycles;
        PlanDetails = planDetails;
    }
}
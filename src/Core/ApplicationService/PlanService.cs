using Core.Interface;
using Core.Model;

namespace Core.ApplicationService;

public class PlanService
{
    private readonly IPlanRepository planRepository;

    public PlanService(IPlanRepository planRepository)
    {
        this.planRepository = planRepository;
    }

    public ICollection<Plan> ListEligiblePlans(string fiscalIdentificationNumber)
    {
        var plans = planRepository.ListPlans(fiscalIdentificationNumber);

        // Business rules to filter eligible plans

        return plans;
    }
}
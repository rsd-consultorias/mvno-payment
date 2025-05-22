using Core.ApplicationService;
using Core.Interface;
using Core.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Core.Tests;

[TestClass]
public class PlanServiceTest
{

    private readonly IPlanRepository planRepository;
    private readonly PlanService planService;

    public PlanServiceTest()
    {
        planRepository = new PlanRepositoryMock();
        planService = new PlanService(planRepository);
    }

    [TestMethod("List eligible plans only")]
    public void ListEligiblePlan()
    {
        var plan = planRepository.FindById(Guid.NewGuid());
        var plans = planService.ListEligiblePlans("12345678901");

        Assert.IsNotNull(plans, "A plan should be found");
        Assert.AreEqual(1, plans.First().PlanDetails.Count, "Should have one detail");
        Assert.IsNotNull(plans.First().PlanDetails.First().Name, "Name shouldn't be null");
        Assert.IsNotNull(plans.First().PlanDetails.First().Description, "Description shouldn't be null");
    }

    #region Mocks
    private class PlanRepositoryMock : IPlanRepository
    {
        public Plan FindById(Guid id)
        {
            var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], []);
            plan.PlanDetails = [
                new PlanDetailVO {
                    Name = "Detail 1", Description = "Description 1"
                }
            ];

            return plan;
        }

        public ICollection<Plan> ListPlans(string fiscalIdentificationNumber)
        {
            // Call external resource to aggregate data
            var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], []);
            plan.PlanDetails = [
                new PlanDetailVO {
                    Name = "Detail 1", Description = "Description 1"
                }
            ];

            return [plan];
        }
    }

    #endregion
}


using Core.ApplicationService;
using Core.Interface;
using Core.Model;

namespace Core.Tests;

[TestClass]
public class BillingServiceTest
{
    private readonly BillingService billingService;
    private readonly IPlanRepository planRepository;
    private readonly ISubscriptionRepository subscriptionRepository;
    public static readonly Guid TEST_ID = Guid.NewGuid();

    public BillingServiceTest()
    {
        planRepository = new PlanRepositoryMock();
        subscriptionRepository = new SubscriptionRepositoryMock();
        billingService = new BillingService(subscriptionRepository, planRepository);
    }

    [TestMethod("Test processing billing event with success")]
    public void ProcessBillingEvent()
    {
        var billingEvent = new BillingEvent
        {
            CorrelationId = TEST_ID,
            Price = 59.90M,
            TimeStamp = DateTime.UtcNow
        };

        for (int month = 0; month < 15; month++)
        {
            billingEvent.Price = month < 3 ? 0M : month < 12 ? 59.90M : 49.90M;
            billingService.ProcessBillingEvent(billingEvent);
        }

        var subscription = subscriptionRepository.FindById(TEST_ID);
        Assert.AreEqual(3, subscription.ExecutionCycles.Count(x => x.TenureType == ETenureType.TRIAL), "Should have 3 months in trial period");
        Assert.AreEqual(9, subscription.ExecutionCycles.Count(x => x.TenureType == ETenureType.REGULAR), "Should have 9 months in regular period");
        Assert.AreEqual(3, subscription.ExecutionCycles.Count(x => x.TenureType == ETenureType.PROMOTIONAL), "Should have 3 months in promotional period");
    }

    #region Mocks
    private class PlanRepositoryMock : IPlanRepository
    {
        public Plan FindById(Guid id)
        {
            var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], []);
            plan.Id = TEST_ID;
            plan.PlanDetails = [
                new PlanDetailVO {
                    Name = "Detail 1", Description = "Description 1"
                }
            ];
            plan.BillingCycles.Add(
                new BillingCycle
                {
                    Currency = "BRL",
                    FrequencyUnit = EFrequencyUnit.MONTH,
                    Sequence = 0,
                    Price = 0,
                    TenureType = ETenureType.TRIAL,
                    TotalCycles = 3
                }
            );
            plan.BillingCycles.Add(
                new BillingCycle
                {
                    Currency = "BRL",
                    FrequencyUnit = EFrequencyUnit.MONTH,
                    Sequence = 1,
                    Price = 59.90M,
                    TenureType = ETenureType.REGULAR,
                    TotalCycles = 9
                }
            );
            plan.BillingCycles.Add(
                new BillingCycle
                {
                    Currency = "BRL",
                    FrequencyUnit = EFrequencyUnit.MONTH,
                    Sequence = 2,
                    Price = 39.90M,
                    TenureType = ETenureType.PROMOTIONAL,
                    TotalCycles = 3
                }
            );

            return plan;
        }

        public ICollection<Plan> ListPlans(string fiscalIdentificationNumber)
        {
            // Call external resource to aggregate data
            var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], [])
            {
                Id = TEST_ID
            };
            plan.BillingCycles.Add(
                new BillingCycle
                {
                    Currency = "BRL",
                    FrequencyUnit = EFrequencyUnit.MONTH,
                    Sequence = 0,
                    Price = 0,
                    TenureType = ETenureType.TRIAL,
                    TotalCycles = 3
                }
            );
            plan.BillingCycles.Add(
                new BillingCycle
                {
                    Currency = "BRL",
                    FrequencyUnit = EFrequencyUnit.MONTH,
                    Sequence = 1,
                    Price = 59.90M,
                    TenureType = ETenureType.REGULAR,
                    TotalCycles = 9
                }
            );
            plan.BillingCycles.Add(
                new BillingCycle
                {
                    Currency = "BRL",
                    FrequencyUnit = EFrequencyUnit.MONTH,
                    Sequence = 2,
                    Price = 39.90M,
                    TenureType = ETenureType.PROMOTIONAL,
                    TotalCycles = 3
                }
            );

            return [plan];
        }
    }

    class SubscriptionRepositoryMock : ISubscriptionRepository
    {
        private Dictionary<Guid, Subscription> databaseMock = [];

        public SubscriptionRepositoryMock()
        {
            databaseMock.Add(TEST_ID, new Subscription
            {
                Id = TEST_ID,
                ExecutionCycles = [],
                StatusChangeNote = "Active",
                Subscriber = new CustomerInfo
                {
                    Addresses = [],
                    EMail = "teste@teste",
                    FirstName = "John",
                    LastName = "Doe",
                    FiscalIdentificationNumber = "12345678901",
                    PaymentMehtods = [],
                    Phone = "+5511999999999",
                    BirthDate = DateOnly.MinValue,
                    Id = Guid.NewGuid()
                },
                PaymentInfo = new PaymentInfoVO
                {
                    PaymentMethod = new CreditCard
                    {
                        BillingAddress = new Address
                        {
                            Country = "",
                            Locality = "",
                            PostalCode = "",
                            Region = "",
                            Street = ""
                        },
                        Brand = "",
                        CardToken = "",
                        Expiry = "",
                        LastDigits = "",
                        NameOnCard = ""
                    },
                    PaymentPlatform = "",
                    PlatformPayerId = "",
                    PlatformTransactionId = "",
                    Approved = true
                },
                PlanId = TEST_ID,
                StartTime = DateTime.UtcNow,
                State = ESubscriptionState.ACTIVE,
                StatusUpdateTime = DateTime.UtcNow
            });
        }

        public Subscription FindById(Guid Id)
        {
            var subscription = new Subscription
            {
                Subscriber = databaseMock[Id].Subscriber,
                ExecutionCycles = databaseMock[Id].ExecutionCycles,
                StatusChangeNote = databaseMock[Id].StatusChangeNote,
                Id = databaseMock[Id].Id,
                PaymentInfo = databaseMock[Id].PaymentInfo,
                PlanId = databaseMock[Id].PlanId,
                StartTime = databaseMock[Id].StartTime,
                State = databaseMock[Id].State,
                StatusUpdateTime = databaseMock[Id].StatusUpdateTime
            };
            return subscription;
        }

        public void Save(Subscription subscription)
        {
            if (databaseMock.ContainsKey(subscription.Id))
            {
                databaseMock[subscription.Id] = subscription;
            }
            else
            {
                databaseMock.Add(subscription.Id, subscription);
            }
        }
    }
    #endregion
}
using System.Data.Common;
using Core.DomainService;
using Core.Interface;
using Core.Model;

namespace Core.Tests;

[TestClass]
public class CheckoutProcessTest
{
    private readonly ISubscriptionRepository subscriptionRepository;
    private readonly CheckoutService checkoutService;
    private readonly IMVNOService mvnoService;

    public CheckoutProcessTest()
    {
        subscriptionRepository = new SubscriptionRepositoryMock();
        mvnoService = new MVNOServiceMock();
        checkoutService = new CheckoutService(subscriptionRepository, mvnoService);
    }

    [TestMethod("Complete process with success result")]
    public void CompleteProcessWithSuccess()
    {
        // Mock plan
        var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], []);
        plan.Id = Guid.NewGuid();

        // 1 - Initialize checkout
        var checkoutResponse = checkoutService.Initialize("12345678901", plan);
        var subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);

        Assert.IsNotNull(checkoutResponse.Subscription, "Subscription should be created");
        Assert.IsNotNull(subscription, "Subscription should be persisted");
        Assert.AreEqual(ESubscriptionState.INITIALIZED, subscription.State, "Subscription state should be INITIALIZED");
        Assert.AreEqual(ESubscriptionState.INITIALIZED, checkoutResponse.Subscription!.State, "Subscription state should be INITIALIZED");

        // 2 - Add personal data
        var personalData = new PersonalDataVO
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.Parse("1984-08-08"),
            EMail = "teste@teste",
            Phone = "+5511999999999"
        };

        checkoutResponse = checkoutService.AddPersonalData(subscription.Id, personalData);
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.PERSONAL_DATA_ADDED, subscription.State, "Subscription state should be PERSONAL_DATA_ADDED");
        Assert.AreEqual(ESubscriptionState.PERSONAL_DATA_ADDED, checkoutResponse.Subscription!.State, "Subscription state should be PERSONAL_DATA_ADDED");

        Assert.AreEqual("John", subscription.Subscriber.FirstName, "Personal data should be persisted");
        Assert.AreEqual("Doe", subscription.Subscriber.LastName, "Personal data should be persisted");
        Assert.AreEqual("teste@teste", subscription.Subscriber.EMail, "Personal data should be persisted");
        Assert.AreEqual("+5511999999999", subscription.Subscriber.Phone, "Personal data should be persisted");
        Assert.AreEqual(DateOnly.Parse("1984-08-08"), subscription.Subscriber.BirthDate, "Personal data should be persisted");

        // 3 - Add billing address
        var address = new Address
        {
            Country = "BR",
            Locality = "Sao Paulo",
            PostalCode = "99999999",
            Region = "Sao Paulo",
            Street = "Rua de Teste, 404",
            BillingAddres = true
        };

        checkoutResponse = checkoutService.AddBillingAddress(subscription.Id, address);
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.BILLING_ADDRESS_ADDED, subscription.State, "Subscription state should be BILLING_ADDRESS_ADDED");
        Assert.AreEqual(ESubscriptionState.BILLING_ADDRESS_ADDED, checkoutResponse.Subscription!.State, "Subscription state should be BILLING_ADDRESS_ADDED");

        // 4 - Request payment approval
        var creditCard = new CreditCard
        {
            CardToken = "2434234324243242342343243242342423",
            BillingAddress = address,
            Brand = "VISA",
            Expiry = "12/2037",
            LastDigits = "1234",
            NameOnCard = "John Doe"
        };

        checkoutResponse = checkoutService.RequestPaymentApproval(subscription.Id, creditCard,
        (subscription) => new PaymentInfoVO
        {
            PaymentMethod = creditCard,
            PaymentPlatform = "PayPal",
            PlatformPayerId = "1234",
            PlatformTransactionId = "1234",
            Approved = true
        });
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.PAYMENT_APPROVED, subscription.State, "Subscription state should be PAYMENT_APPROVED");
        Assert.AreEqual(ESubscriptionState.PAYMENT_APPROVED, checkoutResponse.Subscription!.State, "Subscription state should be PAYMENT_APPROVED");

        // 5 - Activate the service
        var activateParams = new MVNOActivateDTO() { DeviceId = "1" };
        checkoutResponse = checkoutService.RequestServiceActivation(subscription.Id,
        () => mvnoService.Activate(activateParams));
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.ACTIVE, subscription.State, "Subscription state should be ACTIVE");
        Assert.AreEqual(ESubscriptionState.ACTIVE, checkoutResponse.Subscription!.State, "Subscription state should be ACTIVE");
    }

    [TestMethod("Complete process with payment failure result")]
    public void CompleteProcessWithPaymentFilure()
    {
        // Mock plan
        var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], []);
        plan.Id = Guid.NewGuid();

        // 1 - Initialize checkout
        var checkoutResponse = checkoutService.Initialize("12345678901", plan);
        var subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);

        Assert.IsNotNull(checkoutResponse.Subscription, "Subscription should be created");
        Assert.IsNotNull(subscription, "Subscription should be persisted");
        Assert.AreEqual(ESubscriptionState.INITIALIZED, subscription.State, "Subscription state should be INITIALIZED");
        Assert.AreEqual(ESubscriptionState.INITIALIZED, checkoutResponse.Subscription!.State, "Subscription state should be INITIALIZED");

        // 2 - Add personal data
        var personalData = new PersonalDataVO
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.Parse("1984-08-08"),
            EMail = "teste@teste",
            Phone = "+5511999999999"
        };

        checkoutResponse = checkoutService.AddPersonalData(subscription.Id, personalData);
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.PERSONAL_DATA_ADDED, subscription.State, "Subscription state should be PERSONAL_DATA_ADDED");
        Assert.AreEqual(ESubscriptionState.PERSONAL_DATA_ADDED, checkoutResponse.Subscription!.State, "Subscription state should be PERSONAL_DATA_ADDED");

        Assert.AreEqual("John", subscription.Subscriber.FirstName, "Personal data should be persisted");
        Assert.AreEqual("Doe", subscription.Subscriber.LastName, "Personal data should be persisted");
        Assert.AreEqual("teste@teste", subscription.Subscriber.EMail, "Personal data should be persisted");
        Assert.AreEqual("+5511999999999", subscription.Subscriber.Phone, "Personal data should be persisted");
        Assert.AreEqual(DateOnly.Parse("1984-08-08"), subscription.Subscriber.BirthDate, "Personal data should be persisted");

        // 3 - Add billing address
        var address = new Address
        {
            Country = "BR",
            Locality = "Sao Paulo",
            PostalCode = "9999999",
            Region = "Sao Paulo",
            Street = "Rua de Teste, 404",
            BillingAddres = true
        };

        checkoutResponse = checkoutService.AddBillingAddress(subscription.Id, address);
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.BILLING_ADDRESS_ADDED, subscription.State, "Subscription state should be BILLING_ADDRESS_ADDED");
        Assert.AreEqual(ESubscriptionState.BILLING_ADDRESS_ADDED, checkoutResponse.Subscription!.State, "Subscription state should be BILLING_ADDRESS_ADDED");

        // 4 - Request payment approval
        var creditCard = new CreditCard
        {
            CardToken = "2434234324243242342343243242342423",
            BillingAddress = address,
            Brand = "VISA",
            Expiry = "12/2037",
            LastDigits = "1234",
            NameOnCard = "John Doe"
        };

        checkoutResponse = checkoutService.RequestPaymentApproval(subscription.Id, creditCard,
        (subscription) => new PaymentInfoVO
        {
            PaymentMethod = creditCard,
            PaymentPlatform = "PayPal",
            PlatformPayerId = "1234",
            PlatformTransactionId = "1234",
            Approved = false
        });
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.PAYMENT_FAILURE, subscription.State, "Subscription state should be PAYMENT_FAILURE");
        Assert.AreEqual(ESubscriptionState.PAYMENT_FAILURE, checkoutResponse.Subscription!.State, "Subscription state should be PAYMENT_FAILURE");
    }

    [TestMethod("Complete process with activation error result")]
    public void CompleteProcessWithActivationError()
    {
        // Mock plan
        var plan = new Plan("SKUTEST", "Plan Test", "Plan for testing purposes", [], []);
        plan.Id = Guid.NewGuid();

        // 1 - Initialize checkout
        var checkoutResponse = checkoutService.Initialize("12345678901", plan);
        var subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);

        Assert.IsNotNull(checkoutResponse.Subscription, "Subscription should be created");
        Assert.IsNotNull(subscription, "Subscription should be persisted");
        Assert.AreEqual(ESubscriptionState.INITIALIZED, subscription.State, "Subscription state should be INITIALIZED");
        Assert.AreEqual(ESubscriptionState.INITIALIZED, checkoutResponse.Subscription!.State, "Subscription state should be INITIALIZED");

        // 2 - Add personal data
        var personalData = new PersonalDataVO
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.Parse("1984-08-08"),
            EMail = "teste@teste",
            Phone = "+5511999999999"
        };

        checkoutResponse = checkoutService.AddPersonalData(subscription.Id, personalData);
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.PERSONAL_DATA_ADDED, subscription.State, "Subscription state should be PERSONAL_DATA_ADDED");
        Assert.AreEqual(ESubscriptionState.PERSONAL_DATA_ADDED, checkoutResponse.Subscription!.State, "Subscription state should be PERSONAL_DATA_ADDED");

        Assert.AreEqual("John", subscription.Subscriber.FirstName, "Personal data should be persisted");
        Assert.AreEqual("Doe", subscription.Subscriber.LastName, "Personal data should be persisted");
        Assert.AreEqual("teste@teste", subscription.Subscriber.EMail, "Personal data should be persisted");
        Assert.AreEqual("+5511999999999", subscription.Subscriber.Phone, "Personal data should be persisted");
        Assert.AreEqual(DateOnly.Parse("1984-08-08"), subscription.Subscriber.BirthDate, "Personal data should be persisted");

        // 3 - Add billing address
        var address = new Address
        {
            Country = "BR",
            Locality = "Sao Paulo",
            PostalCode = "9999999",
            Region = "Sao Paulo",
            Street = "Rua de Teste, 404",
            BillingAddres = true
        };

        checkoutResponse = checkoutService.AddBillingAddress(subscription.Id, address);
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.BILLING_ADDRESS_ADDED, subscription.State, "Subscription state should be BILLING_ADDRESS_ADDED");
        Assert.AreEqual(ESubscriptionState.BILLING_ADDRESS_ADDED, checkoutResponse.Subscription!.State, "Subscription state should be BILLING_ADDRESS_ADDED");

        // 4 - Request payment approval
        var creditCard = new CreditCard
        {
            CardToken = "2434234324243242342343243242342423",
            BillingAddress = address,
            Brand = "VISA",
            Expiry = "12/2037",
            LastDigits = "1234",
            NameOnCard = "John Doe"
        };

        checkoutResponse = checkoutService.RequestPaymentApproval(subscription.Id, creditCard,
        (subscription) => new PaymentInfoVO
        {
            PaymentMethod = creditCard,
            PaymentPlatform = "PayPal",
            PlatformPayerId = "1234",
            PlatformTransactionId = "1234",
            Approved = true
        });
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.PAYMENT_APPROVED, subscription.State, "Subscription state should be PAYMENT_APPROVED");
        Assert.AreEqual(ESubscriptionState.PAYMENT_APPROVED, checkoutResponse.Subscription!.State, "Subscription state should be PAYMENT_APPROVED");

        // 5 - Activate the service
        var activateParams = new MVNOActivateDTO() { DeviceId = "2" };
        checkoutResponse = checkoutService.RequestServiceActivation(subscription.Id,
        () => mvnoService.Activate(activateParams));
        subscription = subscriptionRepository.FindById(checkoutResponse.Subscription!.Id);
        Assert.AreEqual(ESubscriptionState.ACTIVATION_ERROR, subscription.State, "Subscription state should be ACTIVATION_ERROR");
        Assert.AreEqual(ESubscriptionState.ACTIVATION_ERROR, checkoutResponse.Subscription!.State, "Subscription state should be ACTIVATION_ERROR");
    }

    #region Mocks
    class MVNOServiceMock : IMVNOService
    {
        public ServiceInfoVO Activate(MVNOActivateDTO activateParams)
        {
            return new ServiceInfoVO()
            {
                Active = activateParams.DeviceId.Equals("1"),
                CorrelationId = Guid.NewGuid()
            };
        }

        public ServiceInfoVO Cancel(object cancelParams)
        {
            throw new NotImplementedException();
        }

        public ServiceInfoVO Suspend(object suspendParams)
        {
            throw new NotImplementedException();
        }

        public ServiceInfoVO Update(object updateParams)
        {
            throw new NotImplementedException();
        }
    }

    class SubscriptionRepositoryMock : ISubscriptionRepository
    {
        private Dictionary<Guid, Subscription> databaseMock = [];

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
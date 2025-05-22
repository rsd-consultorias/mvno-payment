using Core.ApplicationService;
using Core.Interface;
using Core.Model;

namespace Core.DomainService;

public class CheckoutService
{
    private readonly ISubscriptionRepository subscriptionRepository;
    private readonly IMVNOService mvnoService;

    public CheckoutService(ISubscriptionRepository subscriptionRepository, IMVNOService mvnoService)
    {
        this.subscriptionRepository = subscriptionRepository;
        this.mvnoService = mvnoService;
    }

    public CheckoutProcessResponse Initialize(string fiscalIdentificationNumber, Plan plan)
    {
        var customer = new CustomerInfo
        {
            FiscalIdentificationNumber = fiscalIdentificationNumber,
            FirstName = "",
            LastName = "",
            BirthDate = DateOnly.MinValue,
            Phone = "",
            EMail = "",
            Addresses = [],
            PaymentMehtods = []
        };
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            PlanId = plan.Id,
            Subscriber = customer,
            ExecutionCycles = [],
            StatusChangeNote = "Process initialized",
            StartTime = DateTime.UtcNow,
            State = ESubscriptionState.INITIALIZED,
        };

        subscriptionRepository.Save(subscription);

        return new CheckoutProcessResponse { Success = true, Subscription = subscription };
    }

    public CheckoutProcessResponse AddPersonalData(Guid subscriptionId, PersonalDataVO personalData)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.PERSONAL_DATA_ADDED;
        subscription.StatusChangeNote = "Personal data added";
        subscription.Subscriber.FirstName = personalData.FirstName!;
        subscription.Subscriber.LastName = personalData.LastName!;
        subscription.Subscriber.EMail = personalData.EMail!;
        subscription.Subscriber.Phone = personalData.Phone!;
        subscription.Subscriber.BirthDate = personalData.BirthDate!;

        subscriptionRepository.Save(subscription);

        return new CheckoutProcessResponse { Success = true, Subscription = subscription };
    }

    public CheckoutProcessResponse AddBillingAddress(Guid subscriptionId, Address billingAddress)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.BILLING_ADDRESS_ADDED;
        subscription.StatusChangeNote = "Billing address added";
        billingAddress.BillingAddres = true;
        subscription.Subscriber.Addresses.Add(billingAddress);

        subscriptionRepository.Save(subscription);

        return new CheckoutProcessResponse { Success = true, Subscription = subscription };
    }

    public CheckoutProcessResponse RequestPaymentApproval(Guid subscriptionId, IPaymentMethod paymentMethod, Func<Subscription, PaymentInfoVO> paymentApprovalFunction)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.PAYMENT_APPROVAL_PENDING;
        subscription.StatusChangeNote = "Payment approval sent";
        subscription.Subscriber.PaymentMehtods.Add(paymentMethod);

        subscriptionRepository.Save(subscription);

        // Call delegate to approve payment
        var paymentInfo = paymentApprovalFunction(subscription);

        if (paymentInfo.Approved)
        {
            AddApprovedPaymentInfo(subscriptionId, paymentInfo);
            subscription.State = ESubscriptionState.PAYMENT_APPROVED;
            return new CheckoutProcessResponse { Success = true, Subscription = subscription };
        }
        else
        {
            InformPaymentFailure(subscriptionId, paymentInfo.FailureReason!);
            subscription.State = ESubscriptionState.PAYMENT_FAILURE;
            return new CheckoutProcessResponse { Success = false, Message = paymentInfo.FailureReason, Subscription = subscription };
        }
    }

    private CheckoutProcessResponse AddApprovedPaymentInfo(Guid subscriptionId, PaymentInfoVO paymentInfo)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.PAYMENT_APPROVED;
        subscription.StatusChangeNote = "Payment approved";

        subscription.PaymentInfo = paymentInfo;

        subscriptionRepository.Save(subscription);

        return new CheckoutProcessResponse { Success = true, Subscription = subscription };
    }

    public CheckoutProcessResponse RequestServiceActivation(Guid subscriptionId, Func<ServiceInfoVO> activationFunction)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.ACTIVATION_REQUESTED;
        subscription.StatusChangeNote = "Activation requested";

        // Call delegate to activate the service
        var activationResponse = activationFunction();

        subscriptionRepository.Save(subscription);

        if (activationResponse.Active)
        {
            InformActivationSucess(subscriptionId);
            subscription.State = ESubscriptionState.ACTIVE;
            return new CheckoutProcessResponse { Success = true, Subscription = subscription };
        }
        else
        {
            InformActivationError(subscriptionId, "Error on activate the service");
            subscription.State = ESubscriptionState.ACTIVATION_ERROR;
            return new CheckoutProcessResponse { Success = false, Message = "Error on activate the service", Subscription = subscription };
        }
    }

    private void InformActivationSucess(Guid subscriptionId)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.ACTIVE;
        subscription.StatusChangeNote = "Activation success";

        subscriptionRepository.Save(subscription);
    }

    private void InformPaymentFailure(Guid subscriptionId, string reason)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.PAYMENT_FAILURE;
        subscription.StatusChangeNote = reason;

        subscriptionRepository.Save(subscription);
    }

    private void InformActivationError(Guid subscriptionId, string reason)
    {
        var subscription = subscriptionRepository.FindById(subscriptionId);

        subscription.StatusUpdateTime = DateTime.UtcNow;
        subscription.State = ESubscriptionState.ACTIVATION_ERROR;
        subscription.StatusChangeNote = reason;

        subscriptionRepository.Save(subscription);
    }
}
using Core.Model;

namespace Core.Interface;

public interface ISubscriptionRepository
{
    Subscription FindById(Guid Id);
    void Save(Subscription subscription);
}
using Core.Model;

namespace Core.Interface;

public interface IPlanRepository
{
    ICollection<Plan> ListPlans(string fiscalIdentificationNumber);
    Plan FindById(Guid id);
}
using Core.Model;

namespace Core.Interface;

public interface IMVNOService {
    ServiceInfoVO Activate(MVNOActivateDTO activateParams);
    ServiceInfoVO Cancel(Object cancelParams);
    ServiceInfoVO Suspend(Object suspendParams);
    ServiceInfoVO Update(Object updateParams);
}
using KironTest.DataModel;

namespace KironTest.Service.Contracts
{
    public interface ISecurityService
    {
        Task<string> Authorize(User user);
        Task<int> Register(User user);
    }
}

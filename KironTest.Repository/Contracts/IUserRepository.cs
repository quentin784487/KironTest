using KironTest.DataModel;

namespace KironTest.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userName);
        Task<int> AddUser(User user);
    }
}

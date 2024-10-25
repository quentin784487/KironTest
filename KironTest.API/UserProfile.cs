using AutoMapper;
using KironTest.DataModel;
using KironTest.Shared.ViewModel;

namespace KironTest.API
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>();
        }
    }
}

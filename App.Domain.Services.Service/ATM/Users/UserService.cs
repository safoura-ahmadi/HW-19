using App.Domain.Core.ATM.Users.Data.Repository;
using App.Domain.Core.ATM.Users.Service;

namespace App.Domain.Services.Service.ATM.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public string GetUserName(int userId)
    {
      return _userRepository.GetUserName(userId);
    }
}

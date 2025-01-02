using App.Domain.Core.ATM.Users.AppService;
using App.Domain.Core.ATM.Users.Service;

namespace App.Domain.Services.AppService.ATM.Users;

public class UserAppService : IUserAppService
{
    private readonly IUserService _userService;
    public UserAppService(IUserService userService)
    {
        _userService = userService;
    }
    public string GetName(int userId)
    {
        try
        {
            return _userService.GetUserName(userId);
        }
        catch (Exception ex)
        {
            return $"Database Have Some Errors: {ex.Message}";
        }
    }
}


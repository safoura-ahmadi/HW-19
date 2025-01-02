namespace App.Domain.Core.ATM.Users.Data.Repository;

public interface IUserRepository
{
    string GetUserName(int userId);
}

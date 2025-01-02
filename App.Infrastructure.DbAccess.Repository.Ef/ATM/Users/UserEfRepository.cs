

using App.Domain.Core.ATM.Users.Data.Repository;
using App.Infrastructure.Db.SqlServer.Ef;

namespace App.Infrastructure.DbAccess.Repository.Ef.ATM.Users;

public class UserEfRepository : IUserRepository
{
    private readonly AppDbContext _Context;
    public UserEfRepository()
    {
        _Context = new AppDbContext();
    }
    public string GetUserName(int userId)
    {
        return _Context.Users.Where(u => u.Id == userId)
             .Select(u => u.Name)
             .First();

    }
}

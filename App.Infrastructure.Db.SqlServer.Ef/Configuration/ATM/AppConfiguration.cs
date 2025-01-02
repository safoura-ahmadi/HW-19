using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM;

public static class AppConfiguration
{
    public readonly static string ConnectionString;
    static AppConfiguration()
    {
        ConnectionString = "Data Source = WIN10\\SQLEXPRESS; Initial Catalog = BankDb; User ID = sa; Password = 246850; TrustServerCertificate = True; Encrypt = True";
    }
}

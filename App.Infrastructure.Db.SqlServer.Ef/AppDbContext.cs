using App.Domain.Core.ATM.Cards.Entitiy;
using App.Domain.Core.ATM.TransActions.Entitiy;
using App.Domain.Core.ATM.Users.Entitiy;
using App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM;
using App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM.Cards;
using App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM.TransActions;
using App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Db.SqlServer.Ef;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(AppConfiguration.ConnectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CardConfiguration());
        modelBuilder.ApplyConfiguration(new TransActionConfiguration());
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<MyTransAction> TransActions { get; set; }

}

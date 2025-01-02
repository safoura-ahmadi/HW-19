using App.Domain.Core.ATM.Users.Entitiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM.Users;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Name)
            .HasColumnType("nvarchar");

        builder.HasMany(u => u.Cards)
            .WithOne(c => c.user)
            .HasForeignKey(c => c.UserId);

        builder.HasData
        (

            new User()
            {
                Id = 1,
                Name = "safoura",
                Email = "safoora.ahmadiasl@gmail.com",
                Age = 21,
                Birthday = new(2003, 10, 2)

            },
            new User()
            {
                Id = 2,
                Name = "tahoura",
                Age = 1,
                Birthday = new(2023, 12, 10)
            }

        );
    }
}


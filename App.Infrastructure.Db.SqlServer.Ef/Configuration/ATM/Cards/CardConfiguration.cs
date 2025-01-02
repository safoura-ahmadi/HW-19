using App.Domain.Core.ATM.Cards.Entitiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM.Cards;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.Property(c => c.Number)
            .HasColumnType("varchar");

        builder.Property(c => c.HolderName)
            .HasColumnType("nvarchar");

        builder.Property(c => c.Password)
            .HasColumnType("varchar");

        builder.HasOne(c => c.user)
            .WithMany(u => u.Cards)
            .HasForeignKey(C => C.UserId);

        builder.HasMany(c => c.SourceTransactions)
            .WithOne(t => t.SourceCard)
            .HasForeignKey(t => t.SourceCardId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.DestinationTransactions)
            .WithOne(t => t.DestinationCard)
            .HasForeignKey(t => t.DestinationCardId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData
        (

                new Card()
                {
                    Id = 1,
                    Number = "6037997411329676",
                    Password = "1234",
                    Balance = 200,
                    UserId = 1,
                    IsActive = true
                },
                new Card()
                {
                    Id = 2,
                    Number = "6037997411329677",
                    Password = "1234",
                    Balance = 200.5f,
                    UserId = 2,
                    IsActive = true
                },
                 new Card()
                 {
                     Id = 3,
                     Number = "6037997411329678",
                     Password = "1234",
                     Balance = 500.50f,
                     UserId = 1,
                     IsActive = true
                 }

        );
    }
}

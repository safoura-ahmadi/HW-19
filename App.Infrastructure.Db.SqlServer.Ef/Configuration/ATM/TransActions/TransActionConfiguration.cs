using App.Domain.Core.ATM.TransActions.Entitiy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Db.SqlServer.Ef.Configuration.ATM.TransActions;

public class TransActionConfiguration : IEntityTypeConfiguration<MyTransAction>
{
    public void Configure(EntityTypeBuilder<MyTransAction> builder)
    {
        builder.Property(t => t.SourceCardNumber)
            .HasColumnType("varchar");

        builder.Property(t => t.DestinationCardNUmber)
            .HasColumnType("varchar");

        builder.HasOne(t => t.SourceCard)
            .WithMany(sc => sc.SourceTransactions)
            .HasForeignKey(t => t.SourceCardId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.DestinationCard)
            .WithMany(dc => dc.DestinationTransactions)
            .HasForeignKey(t => t.DestinationCardId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


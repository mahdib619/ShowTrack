using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShowTrack.Data.Extensions;
using ShowTrack.Domain.Entities;

namespace ShowTrack.Data.TypeConfigurations;

internal sealed class ShowTypeConfiguration : IEntityTypeConfiguration<Show>
{
    public void Configure(EntityTypeBuilder<Show> builder)
    {
        builder.Property(e => e.Id).IsRequiredGuid();

        builder.Property(e => e.Title).HasMaxLength(100)
                                      .IsRequired();

        builder.Property(e => e.UserId).HasMaxLength(450);

        builder.Property(e => e.CurrentSeason).HasMaxLength(10)
                                              .IsRequired();

        builder.Property(e => e.DatePinned).HasDefaultValue(DateTime.MinValue);

        builder.HasOne(e => e.Schedule)
               .WithOne(e => e.Show)
               .HasForeignKey<ShowSchedule>(e => e.ShowId)
               .IsRequired();
    }
}

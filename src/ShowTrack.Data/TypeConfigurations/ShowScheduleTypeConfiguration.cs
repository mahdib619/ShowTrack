using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShowTrack.Data.Extensions;
using ShowTrack.Domain.Entities;

namespace ShowTrack.Data.TypeConfigurations;

internal sealed class ShowScheduleTypeConfiguration : IEntityTypeConfiguration<ShowSchedule>
{
    public void Configure(EntityTypeBuilder<ShowSchedule> builder)
    {
        builder.Property(e => e.Id).IsRequiredGuid();

        builder.Property(e => e.ShowId).IsRequiredGuid();

        builder.Property(e => e.Season).HasMaxLength(10)
                                       .IsRequired();
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShowTrack.Data.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<string> IsRequiredGuid(this PropertyBuilder<string> builder)
    {
        return builder.IsUnicode(false)
                      .HasMaxLength(36)
                      .IsRequired();
    }

    public static PropertyBuilder<string?> IsGuid(this PropertyBuilder<string?> builder)
    {
        return builder.IsUnicode(false)
                      .HasMaxLength(36);
    }
}

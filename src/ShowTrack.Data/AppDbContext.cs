using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShowTrack.Domain.Entities;

namespace ShowTrack.Data;

public sealed class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public required DbSet<Show> Shows { get; init; }
    public required DbSet<ShowSchedule> ShowSchedules { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        if (Database.ProviderName?.Contains("PostgreSQL",StringComparison.CurrentCultureIgnoreCase) is true)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeUtcConverter>();

        }
    }
}

file class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeUtcConverter() : base(dt => ToProvider(dt), dt => FromProvider(dt))
    {
    }

    private static DateTime ToProvider(DateTime dateTime)
    {
        if (dateTime.Kind is not DateTimeKind.Utc)
        {
            dateTime = dateTime.ToUniversalTime();
        }

        return dateTime;
    }

    private static DateTime FromProvider(DateTime dateTime)
    {
        if (dateTime.Kind is not DateTimeKind.Local)
        {
            dateTime = dateTime.ToLocalTime();
        }

        return dateTime;
    }
}

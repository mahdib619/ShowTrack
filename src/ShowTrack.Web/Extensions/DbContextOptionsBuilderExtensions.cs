using Microsoft.EntityFrameworkCore;

namespace ShowTrack.Web.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    private const string MSSQL = "mssql";
    private const string NPGSQL = "npgsql";

    public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string? dbProvider, IConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(dbProvider), dbProvider);

        dbProvider = dbProvider!.ToLower();
        Console.WriteLine(dbProvider);

        return dbProvider switch
        {
            MSSQL => builder.UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString") ?? configuration.GetConnectionString("DefaultConnectionString"), 
                opt =>
                {
                    opt.MigrationsAssembly("SqlServerMigrator");
                }
            ),

            NPGSQL => builder.UseNpgsql(configuration.GetConnectionString("PostgresqlConnectionString") ?? configuration.GetConnectionString("DefaultConnectionString"), 
                opt =>
                {
                    opt.MigrationsAssembly("PostgresqlMigrator");
                }
            ),

            _ => throw new InvalidOperationException($"Unsupported provider: {dbProvider}")
        };
    }
}
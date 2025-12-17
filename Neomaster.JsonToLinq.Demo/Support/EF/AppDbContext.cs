using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class AppDbContext : DbContext
{
  public AppDbContext(string connectionString = null)
  {
    ConnectionString = connectionString ?? DefaultConnectionString;
  }

  public static string DefaultConnectionString { get; set; }

  public string ConnectionString { get; }

  public Log Log { get; } = new();

  public DbSet<User> Users => Set<User>();

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
    builder.UseNpgsql(ConnectionString);
    builder.UseLoggerFactory(LoggerFactory.Create(builder =>
    {
      builder.ClearProviders();
      builder.AddProvider(new EFLoggerProvider(Log));
    }));
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}

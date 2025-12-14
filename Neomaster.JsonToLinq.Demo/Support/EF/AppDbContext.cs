using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class AppDbContext(DbContextOptions<AppDbContext> options)
  : DbContext(options)
{
  public EFLog Log { get; } = new();

  public DbSet<User> Users => Set<User>();

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
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

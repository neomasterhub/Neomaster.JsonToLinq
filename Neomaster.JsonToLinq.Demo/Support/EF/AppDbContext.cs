using Microsoft.EntityFrameworkCore;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class AppDbContext(DbContextOptions<AppDbContext> options)
  : DbContext(options)
{
  public DbSet<User> Users => Set<User>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}

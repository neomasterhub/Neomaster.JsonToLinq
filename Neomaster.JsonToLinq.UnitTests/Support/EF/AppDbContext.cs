using Microsoft.EntityFrameworkCore;

namespace Neomaster.JsonToLinq.UnitTests;

public class AppDbContext : DbContext
{
  public DbSet<User> Users => Set<User>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=JsonToLinqTest;Username=postgres;Password=postgres");
  }
}

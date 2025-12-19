using Bogus;
using Microsoft.EntityFrameworkCore;
using Neomaster.JsonToLinq.UnitTests;

namespace Neomaster.JsonToLinq.Demo;

internal class DataService()
{
  public void Prepare(Log log)
  {
    using var dbContext = new AppDbContext();

    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    var dt1 = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var dt2 = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    Randomizer.Seed = new Random(1);

    dbContext.Users.AddRange(new Faker<User>()
      .RuleFor(u => u.Email, f => f.Internet.Email())
      .RuleFor(u => u.FirstName, f => f.Name.FirstName())
      .RuleFor(u => u.LastName, f => f.Name.LastName())
      .RuleFor(u => u.MiddleName, f => f.Name.FirstName())
      .RuleFor(u => u.Balance, f => f.Random.Decimal(-100, 10000))
      .RuleFor(u => u.LastVisitAt, f => f.IndexFaker % 100 == 0 ? null : f.Date.Between(dt1, dt2))
      .Generate(10_000));

    dbContext.SaveChanges();

    log.Add("Data is ready!");
  }
}

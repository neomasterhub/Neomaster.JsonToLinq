using Microsoft.EntityFrameworkCore;

namespace Neomaster.JsonToLinq.Demo;

internal class DataService(AppDbContext dbContext)
{
  public void Prepare()
  {
    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    // TODO: Fill tables

    Console.WriteLine();
    dbContext.Log.Print();
  }
}

using Microsoft.EntityFrameworkCore;

namespace Neomaster.JsonToLinq.Demo;

internal class DataService(AppDbContext dbContext)
{
  public void Prepare(Log log)
  {
    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    // TODO: Fill tables

    dbContext.Log.CopyTo(log);
  }
}

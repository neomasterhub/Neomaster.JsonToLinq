namespace Neomaster.JsonToLinq.UnitTests;

public record User
{
  public int Id { get; set; }
  public decimal Balance { get; set; }
  public DateTime? LastVisitAt { get; set; }
}

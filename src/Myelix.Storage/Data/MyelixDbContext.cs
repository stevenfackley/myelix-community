using Microsoft.EntityFrameworkCore;

namespace Myelix.Storage.Data;

public class MyelixDbContext : DbContext
{
    public DbSet<BaselineEntity> Baselines => Set<BaselineEntity>();
    public DbSet<ScoreEntity> Scores => Set<ScoreEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=myelix.db");
    }
}

public class BaselineEntity
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public DateTimeOffset Created { get; set; }
}

public class ScoreEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public double Score { get; set; }
    public double Confidence { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

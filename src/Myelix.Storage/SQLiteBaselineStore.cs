using Myelix.Core.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Myelix.Storage.Data;

public class SQLiteBaselineStore : IBaselineStore
{
    private readonly MyelixDbContext _db;

    public SQLiteBaselineStore(MyelixDbContext db)
    {
        _db = db;
    }

    public async Task<float[]> GetBaselineAsync(string userId, CancellationToken ct = default)
    {
        var entity = await _db.Baselines.FirstOrDefaultAsync(b => b.UserId == userId, ct);
        if (entity == null) return Array.Empty<float>();
        
        var floats = new float[entity.Data.Length / 4];
        Buffer.BlockCopy(entity.Data, 0, floats, 0, entity.Data.Length);
        return floats;
    }

    public async Task SaveBaselineAsync(string userId, float[] baseline, CancellationToken ct = default)
    {
        var entity = await _db.Baselines.FirstOrDefaultAsync(b => b.UserId == userId, ct);
        var bytes = new byte[baseline.Length * 4];
        Buffer.BlockCopy(baseline, 0, bytes, 0, bytes.Length);

        if (entity == null)
        {
            _db.Baselines.Add(new BaselineEntity { UserId = userId, Data = bytes, Created = DateTimeOffset.UtcNow });
        }
        else
        {
            entity.Data = bytes;
        }
        await _db.SaveChangesAsync(ct);
    }
}

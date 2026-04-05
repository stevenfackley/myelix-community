using Myelix.Core.Contracts.Interfaces;
using Myelix.Core.Contracts.Models;

namespace Myelix.Baseline.Services;

public class BaselineManager
{
    private readonly IBaselineStore _store;
    private readonly IDriftScorer _scorer;
    private readonly IFeatureExtractor _extractor;

    public BaselineManager(IBaselineStore store, IDriftScorer scorer, IFeatureExtractor extractor)
    {
        _store = store;
        _scorer = scorer;
        _extractor = extractor;
    }

    public async Task<ResilienceScoreSnapshot> ProcessSamplesAsync(string userId, IEnumerable<ISensorSample> samples)
    {
        var currentFeatures = _extractor.ExtractFeatures(samples);
        var baseline = await _store.GetBaselineAsync(userId);
        
        if (baseline == null || baseline.Length == 0)
        {
            // First time baseline creation
            await _store.SaveBaselineAsync(userId, currentFeatures);
            return CreateSnapshot(100.0, 0.5); // Initial score
        }

        var drift = _scorer.CalculateDrift(currentFeatures, baseline);
        var score = Math.Max(0, 100.0 - (drift * 100.0));
        
        return CreateSnapshot(score, 0.9);
    }

    private ResilienceScoreSnapshot CreateSnapshot(double score, double confidence)
    {
        return new ResilienceScoreSnapshot(
            score,
            confidence,
            DateTimeOffset.UtcNow,
            new Dictionary<string, double> { { "General", score } },
            new Dictionary<string, string> { { "Version", "1.3.0" } }
        );
    }
}

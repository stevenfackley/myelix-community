using Myelix.Core.Contracts.Interfaces;

namespace Myelix.Sensors.Providers;

public class MockTypingSensorProvider : BaseSensorProvider
{
    public override string Name => "TypingCadence";

    public override async Task StartAsync(CancellationToken ct = default)
    {
        await base.StartAsync(ct);
        _ = SimulateTypingAsync(ct);
    }

    private async Task SimulateTypingAsync(CancellationToken ct)
    {
        var random = new Random();
        while (IsActive && !ct.IsCancellationRequested)
        {
            try 
            {
                await Task.Delay(random.Next(500, 2000), ct);
                var sample = new GenericSensorSample(
                    DateTimeOffset.UtcNow,
                    Name,
                    new Dictionary<string, object> { { "IKD", random.NextDouble() * 100 } }
                );
                OnSampleReceived(sample);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}

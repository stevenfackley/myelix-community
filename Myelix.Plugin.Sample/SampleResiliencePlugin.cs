using Myelix.PluginSdk;
using Myelix.PluginSdk.Interfaces;

namespace Myelix.Plugin.Sample;

public class SampleResiliencePlugin : PluginBase
{
    public override string Id => "com.myelix.sample.plugin";
    public override string Version => "1.0.0";

    public override async Task ExecuteAsync(IPluginContext context)
    {
        var score = await context.GetCurrentResilienceScoreAsync();
        Console.WriteLine($"Sample Plugin executing. Current score: {score}");
    }
}

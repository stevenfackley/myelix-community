using Myelix.Core.Contracts.Interfaces;
using Myelix.Core.Runtime.Services;

namespace Myelix.Inference.Services;

public class OnnxInferenceService
{
    private readonly ModelRegistry _registry;

    public OnnxInferenceService(ModelRegistry registry)
    {
        _registry = registry;
    }

    public async Task<float[]> RunInferenceAsync(string modelId, float[] input)
    {
        var modelPath = await _registry.GetModelPathAsync(modelId);
        
        // Stub: In a real implementation, we would load the ONNX model and run it here.
        // For Phase 1, we just verify the model path exists.
        if (!File.Exists(modelPath)) 
            throw new FileNotFoundException($"Model file not found at {modelPath}");
        
        return new float[] { 0.5f, 0.5f }; // Stubbed output
    }
}

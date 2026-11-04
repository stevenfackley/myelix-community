using Myelix.Core.Runtime.Services;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Myelix.Inference.Services;

/// <summary>
/// Runs on-device inference via ONNX Runtime.
///
/// Model variant selection:
///   - Default: fp32 (baseline.onnx) — full precision, production default
///   - Opt-in:  int8 (baseline_int8.onnx) — ~20% smaller, faster on low-end ARM;
///              enable by passing useInt8: true to the constructor or by setting
///              MYELIX_USE_INT8=1 in the process environment
///
/// Input tensor shape:  [1, 8, 128] float32
///   - 8 sensor feature channels (cognitive, motor, autonomic dimensions)
///   - 128 time-step window of normalised sensor readings
///
/// Output tensor shape: [1, 4] float32
///   - [0] cognitive score    (0..1)
///   - [1] motor score        (0..1)
///   - [2] autonomic score    (0..1)
///   - [3] composite score    (0..1, weighted blend of above)
/// </summary>
public sealed class OnnxInferenceService : IDisposable
{
    // Model tensor constants — must match export_onnx.py
    public const int InputChannels = 8;
    public const int TimeSteps = 128;
    public const int OutputDim = 4;
    public static readonly string[] OutputLabels = ["cognitive", "motor", "autonomic", "composite"];

    // Model ID for the fp32 baseline in manifest.json
    private const string Fp32ModelId = "baseline-tcn";
    // Model ID for the INT8 quantized variant in manifest.json
    private const string Int8ModelId = "baseline-tcn-int8";

    private readonly ModelRegistry _registry;
    private readonly bool _useInt8;
    private InferenceSession? _session;
    private string? _loadedModelId;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    /// <param name="registry">Resolves model file paths from manifest.json.</param>
    /// <param name="useInt8">
    ///   When true, loads baseline_int8.onnx instead of baseline.onnx.
    ///   Also toggled by environment variable MYELIX_USE_INT8=1.
    ///   Defaults to false — production always starts on fp32.
    /// </param>
    public OnnxInferenceService(ModelRegistry registry, bool useInt8 = false)
    {
        _registry = registry;
        _useInt8 = useInt8
            || string.Equals(
                Environment.GetEnvironmentVariable("MYELIX_USE_INT8"),
                "1",
                StringComparison.Ordinal);
    }

    /// <summary>
    /// The model variant currently configured (does not load the session eagerly).
    /// </summary>
    public string ActiveModelId => _useInt8 ? Int8ModelId : Fp32ModelId;

    /// <summary>
    /// Run inference on a pre-shaped input tensor.
    /// </summary>
    /// <param name="input">
    ///   Float array of length <see cref="InputChannels"/> × <see cref="TimeSteps"/> (1024 elements).
    ///   Layout: [channel_0_t0, channel_0_t1, ..., channel_1_t0, ...] (channel-first / CHW).
    /// </param>
    /// <returns>Float array of length <see cref="OutputDim"/> = 4.</returns>
    public async Task<float[]> RunInferenceAsync(float[] input)
    {
        if (input.Length != InputChannels * TimeSteps)
            throw new ArgumentException(
                $"Input must have {InputChannels * TimeSteps} elements " +
                $"(got {input.Length}). Layout: [C={InputChannels}, T={TimeSteps}].",
                nameof(input));

        var session = await GetOrCreateSessionAsync();
        return RunInferenceCore(session, input);
    }

    /// <summary>
    /// Convenience overload: accepts a 2-D [channels, timeSteps] array.
    /// </summary>
    public async Task<float[]> RunInferenceAsync(float[,] input)
    {
        if (input.GetLength(0) != InputChannels || input.GetLength(1) != TimeSteps)
            throw new ArgumentException(
                $"Input must be [{InputChannels}, {TimeSteps}] (got [{input.GetLength(0)}, {input.GetLength(1)}]).",
                nameof(input));

        // Flatten to [C * T] row-major
        var flat = new float[InputChannels * TimeSteps];
        Buffer.BlockCopy(input, 0, flat, 0, flat.Length * sizeof(float));
        return await RunInferenceAsync(flat);
    }

    /// <summary>
    /// Legacy overload kept for backward compatibility with existing callers
    /// that pass modelId explicitly. The modelId parameter is ignored — use the
    /// variant configured at construction time instead.
    /// </summary>
    [Obsolete("Pass input directly; modelId is resolved from the variant flag set at construction time.")]
    public async Task<float[]> RunInferenceAsync(string modelId, float[] input)
        => await RunInferenceAsync(input);

    // ─────────────────────────────────────────────────────────────────────────
    // Internal
    // ─────────────────────────────────────────────────────────────────────────

    private static float[] RunInferenceCore(InferenceSession session, float[] input)
    {
        // Build input tensor: shape [1 (batch), C, T]
        var dims = new[] { 1, InputChannels, TimeSteps };
        var tensor = new DenseTensor<float>(input, dims);
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", tensor)
        };

        using var results = session.Run(inputs);
        var outputTensor = results[0].AsTensor<float>();

        var output = new float[OutputDim];
        for (int i = 0; i < OutputDim; i++)
            output[i] = outputTensor[0, i];

        return output;
    }

    private async Task<InferenceSession> GetOrCreateSessionAsync()
    {
        if (_session is not null)
            return _session;

        await _initLock.WaitAsync();
        try
        {
            if (_session is not null)
                return _session;

            var modelId = ActiveModelId;
            var modelPath = await _registry.GetModelPathAsync(modelId);

            if (!File.Exists(modelPath))
                throw new FileNotFoundException(
                    $"ONNX model not found at '{modelPath}'. " +
                    "Ensure model artifacts are bundled with the app.", modelPath);

            var opts = new SessionOptions
            {
                LogSeverityLevel = OrtLoggingLevel.ORT_LOGGING_LEVEL_WARNING,
            };

            // On Android: prefer NNAPI delegate for hardware acceleration
            // (no-op on platforms that don't support it)
#if ANDROID
            opts.AppendExecutionProvider_Nnapi();
#endif

            _session = new InferenceSession(modelPath, opts);
            _loadedModelId = modelId;
            return _session;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public void Dispose()
    {
        _session?.Dispose();
        _initLock.Dispose();
    }
}

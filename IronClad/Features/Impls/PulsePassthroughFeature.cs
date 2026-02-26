using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record PulsePassthroughFeatureSettings
{
    [JsonPropertyName("folderName")]
    public string? FolderName { init; get; }

    [JsonPropertyName("runtimeDir")]
    public string? RuntimeDir { init; get; }
}

public sealed class PulsePassthroughFeature(JsonObject @object) : Feature<PulsePassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.PulsePassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var folderName = FeatureConfiguration.FolderName ?? "pulse";
        var runtimeDir = FeatureConfiguration.RuntimeDir
            ?? Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")
            ?? throw new FeatureConfigurationException<PulsePassthroughFeature>("Unable to determine xdg runtime directory. Consider specifying it explicitly");
        var pulseFolderPath = Path.Combine(runtimeDir, folderName);
        if (Directory.Exists(pulseFolderPath))
            throw new FeatureConfigurationException<PulsePassthroughFeature>($"Folder path {pulseFolderPath} does not exist");
        devContainerBuilder
            .AddMount($"type=bind,src={pulseFolderPath},dst={pulseFolderPath}")
            .WithContainerEnv("XDG_RUNTIME_DIR", runtimeDir);
    }
}
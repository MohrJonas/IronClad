using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;
using Mohr.Jonas.IronClad.Features.Dependence;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record PipewirePassthroughFeatureSettings
{
    [JsonPropertyName("socketName")]
    public string? SocketName { init; get; }

    [JsonPropertyName("runtimeDir")]
    public string? RuntimeDir { init; get; }
}

[RequiresFeature(typeof(UserPassthroughFeature))]
public sealed class PipewirePassthroughFeature(JsonObject @object) : Feature<PipewirePassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.PipewirePassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var socketName = FeatureConfiguration.SocketName ?? "pipewire-0";
        var runtimeDir = FeatureConfiguration.RuntimeDir
            ?? Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")
            ?? throw new FeatureConfigurationException<PipewirePassthroughFeature>("Unable to determine xdg runtime directory. Consider specifying it explicitly");
        var pipewireSocketPath = Path.Combine(runtimeDir, socketName);
        if (!File.Exists(pipewireSocketPath))
            throw new FeatureConfigurationException<PipewirePassthroughFeature>($"Socket path {pipewireSocketPath} does not exist");
        devContainerBuilder
            .AddMount($"type=bind,src={pipewireSocketPath},dst={pipewireSocketPath}")
            .WithContainerEnv("XDG_RUNTIME_DIR", runtimeDir);
    }
}
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record WaylandPassthroughFeatureSettings
{
    [JsonPropertyName("waylandDisplay")]
    public string? WaylandDisplay { init; get; }

    [JsonPropertyName("runtimeDir")]
    public string? RuntimeDir { init; get; }
}

public sealed class WaylandPassthroughFeature(JsonObject @object) : Feature<WaylandPassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.WaylandPassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var waylandDisplay = FeatureConfiguration.WaylandDisplay
            ?? Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")
            ?? throw new FeatureConfigurationException<WaylandPassthroughFeatureSettings>("Unable to determine wayland display to passthrough. Consider specifying it explicitly");
        var runtimeDir = FeatureConfiguration.RuntimeDir
            ?? Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")
            ?? throw new FeatureConfigurationException<WaylandPassthroughFeatureSettings>("Unable to determine xdg runtime directory. Consider specifying it explicitly");
        var waylandSocketPath = Path.Combine(runtimeDir, waylandDisplay);
        if (!File.Exists(waylandSocketPath))
            throw new FeatureConfigurationException<WaylandPassthroughFeatureSettings>($"Socket path {waylandSocketPath} does not exist");
        devContainerBuilder
            .AddMount($"type=bind,src={waylandSocketPath},dst={waylandSocketPath}")
            .WithContainerEnv("WAYLAND_DISPLAY", waylandDisplay)
            .WithContainerEnv("XDG_RUNTIME_DIR", runtimeDir);
    }
}
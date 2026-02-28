using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record X11PassthroughFeatureSettings
{
    [JsonPropertyName("display")]
    public string? Display { init; get; }

    [JsonPropertyName("xauthority")]
    public string? Xauthority { init; get; }
}

public sealed class X11PassthroughFeature(JsonObject @object) : Feature<X11PassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.X11PassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var display = FeatureConfiguration.Display
            ?? Environment.GetEnvironmentVariable("DISPLAY")
            ?? throw new FeatureConfigurationException<X11PassthroughFeature>("Unable to determine display to passthrough. Consider specifying it explicitly");
        var socketFileName = $"X{display.TrimStart(':')}";
        var socketPath = Path.Combine("/tmp", ".X11-unix", socketFileName);
        if (!File.Exists(socketPath))
            throw new FeatureConfigurationException<X11PassthroughFeature>($"Socket path {socketPath} does not exist");

        var xauthority = FeatureConfiguration.Xauthority ?? Environment.GetEnvironmentVariable("XAUTHORITY");

        devContainerBuilder
            .AddMount($"type=bind,src={socketPath},dst={socketPath}")
            .WithContainerEnv("DISPLAY", display);

        if(xauthority != null)
            devContainerBuilder
                .AddMount($"type=bind,src={xauthority},dst={xauthority}")
                .WithContainerEnv("XAUTHORITY", xauthority);
    }
}
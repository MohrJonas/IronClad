using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record GpuPassthroughFeatureSettings
{
    [JsonPropertyName("driPath")]
    public string? DriPath { init; get; }
}

public sealed class GpuPassthroughFeature(JsonObject @object) : Feature<GpuPassthroughFeatureSettings>(@object, SourceGenerationContext.Default.GpuPassthroughFeatureSettings)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var driPath = FeatureConfiguration.DriPath ?? "/dev/dri";
        if (!Directory.Exists(driPath))
            throw new FeatureConfigurationException<GpuPassthroughFeature>("Unable to determine dri path to passthrough. Consider specifying it explicitly");
        devContainerBuilder.AddMount($"type=bind,src={driPath},dst={driPath}");
    }
}
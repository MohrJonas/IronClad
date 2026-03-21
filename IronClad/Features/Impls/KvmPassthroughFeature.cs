using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record KvmPassthroughFeatureSettings
{
    [JsonPropertyName("kvmPath")]
    public string? KvmDevicePath { init; get; }
}

public sealed class KvmPassthroughFeature(JsonObject @object) : Feature<KvmPassthroughFeatureSettings>(@object, SourceGenerationContext.Default.KvmPassthroughFeatureSettings)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var devicePath = FeatureConfiguration.KvmDevicePath ?? "/dev/kvm";
        if (!File.Exists(devicePath))
            throw new FeatureConfigurationException<KvmPassthroughFeature>("Unable to determine kvm device to passthrough. Consider specifying it explicitly");
        devContainerBuilder.AddRunArgs("--device", $"{devicePath}:/dev/kvm");
    }
}
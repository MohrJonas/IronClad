using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record DockerPassthroughFeatureSettings
{
    [JsonPropertyName("socketPath")]
    public string? SocketPath { init; get; }
}

public sealed class DockerPassthroughFeature(JsonObject @object) : Feature<DockerPassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.DockerPassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var socketPath = FeatureConfiguration.SocketPath ?? "/var/run/docker.sock";
        if (!File.Exists(socketPath))
            throw new FeatureConfigurationException<DockerPassthroughFeature>("Unable to determine docker socket to passthrough. Consider specifying it explicitly");
        devContainerBuilder.AddMount($"type=bind,src={socketPath},dst={socketPath}");
    }
}
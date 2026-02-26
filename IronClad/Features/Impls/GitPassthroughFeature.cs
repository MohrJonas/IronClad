using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Mohr.Jonas.IronClad.Exceptions;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record GitPassthroughFeatureSettings
{
    [JsonPropertyName("gitconfigPath")]
    public string? GitconfigPath { init; get; }
}

public sealed class GitPassthroughFeature(JsonObject @object) : Feature<GitPassthroughFeatureSettings>(@object, SourceGenerationContext.Default.GitPassthroughFeatureSettings)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var gitconfigPath = FeatureConfiguration.GitconfigPath ?? Path.Combine(
            Environment.GetEnvironmentVariable("HOME")!,
            ".gitconfig"
        );
        if (!File.Exists(gitconfigPath))
            throw new FeatureConfigurationException<GitPassthroughFeature>("Unable to determine gitconfig to passthrough. Consider specifying it explicitly");

        var containerPath = Path.Combine("/home", "clad", Path.GetFileName(gitconfigPath));
        devContainerBuilder.AddMount($"type=bind,src={gitconfigPath},dst={containerPath}");
    }
}
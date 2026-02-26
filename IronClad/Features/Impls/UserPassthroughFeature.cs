using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Mohr.Jonas.IronClad.Features.Impls;

public sealed record UserPassthroughFeatureSettings
{
    [JsonPropertyName("uid")]
    public int? UserId { init; get; }

    [JsonPropertyName("gid")]
    public int? GroupId { init; get; }
}

public sealed class UserPassthroughFeature(JsonObject @object) : Feature<UserPassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.UserPassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var uid = FeatureConfiguration.UserId ?? CInterop.GetUserId();
        var gid = FeatureConfiguration.GroupId ?? CInterop.GetGroupId();

        var isUsingDockerfile = devContainerBuilder.Container.Image == null;

        if (isUsingDockerfile)
        {
            var oldDockerFilePath = Path.Combine(
                devContainerBuilder.Container.Build!.Context ?? Environment.CurrentDirectory,
                devContainerBuilder.Container.Build!.Dockerfile ?? "Dockerfile"
            );
            var dockerFileContents = File.ReadAllLines(oldDockerFilePath).ToList();

            dockerFileContents.AddRange([
                "RUN groupadd -g {gid} container",
                "RUN useradd -g {gid} -u {uid} container"
            ]);

            var dockerfileName = ".IronClad.dockerfile";
            File.WriteAllLines(Path.Combine(Environment.CurrentDirectory, dockerfileName), dockerFileContents);

            var build = new DevContainerBuildBuilder()
                .WithDockerfile(dockerfileName)
                .Build();
            devContainerBuilder.WithBuild(build);
        }
        else
        {
            var image = devContainerBuilder.Container.Image;
            devContainerBuilder.WithImage(null!);
            var dockerFileContents = $"""
            FROM {image}

            RUN if getent passwd {uid}; then userdel -f $(getent passwd {uid} | cut -d ":" -f 1); fi
            RUN if getent group {gid}; then groupdel -f $(getent group {gid} | cut -d ":" -f 1); fi

            RUN groupadd -g {gid} clad
            RUN useradd -g {gid} -u {uid} clad
            """;
            var dockerfilePath = Path.Combine(Environment.CurrentDirectory, ".ironClad.dockerfile");
            File.WriteAllText(dockerfilePath, dockerFileContents);
            var build = new DevContainerBuildBuilder()
                .WithDockerfile(dockerfilePath)
                .Build();
            devContainerBuilder.WithBuild(build);
        }
        devContainerBuilder.WithContainerUser("clad");
    }
}
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

public sealed class UserPassthroughFeature(JsonObject @object, string cwd) : Feature<UserPassthroughFeatureSettings>(
    @object,
    SourceGenerationContext.Default.UserPassthroughFeatureSettings
)
{
    public override void Apply(DevContainerBuilder devContainerBuilder)
    {
        var uid = FeatureConfiguration.UserId ?? CInterop.GetUserId();
        var gid = FeatureConfiguration.GroupId ?? CInterop.GetGroupId();

        var dockerfileName = ".ironClad.dockerfile";

        var isUsingDockerfile = devContainerBuilder.Container.Image == null;
        if (isUsingDockerfile)
        {
            var oldDockerFilePath = Path.Combine(
                devContainerBuilder.Container.Build!.Context ?? cwd,
                devContainerBuilder.Container.Build!.Dockerfile ?? "Dockerfile"
            );
            var dockerFileContents = File.ReadAllLines(oldDockerFilePath).ToList();

            dockerFileContents.AddRange([
                "RUN if getent passwd {uid}; then userdel -f $(getent passwd {uid} | cut -d \":\" -f 1); fi",
                "RUN if getent group {gid}; then groupdel -f $(getent group {gid} | cut -d \":\" -f 1); fi",
                "RUN groupadd -g {gid} clad",
                "RUN useradd -m -s /bin/bash -g {gid} -u {uid} clad"
            ]);

            var dockerfilePath = Path.Combine(cwd, dockerfileName);
            File.WriteAllLines(dockerfilePath, dockerFileContents);
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
            RUN useradd -m -s /bin/bash -g {gid} -u {uid} clad
            """;
            
            var dockerfilePath = Path.Combine(cwd, dockerfileName);
            File.WriteAllText(dockerfilePath, dockerFileContents);
            var build = new DevContainerBuildBuilder()
                .WithDockerfile(dockerfileName)
                .Build();
            devContainerBuilder.WithBuild(build);
        }
        devContainerBuilder.WithContainerUser("clad");
    }
}
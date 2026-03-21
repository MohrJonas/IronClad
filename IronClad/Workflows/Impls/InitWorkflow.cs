using System.Text.Json;
using System.Text.Json.Nodes;
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class InitWorkflow(ILogger logger, string? cwd, string? configPath) : IWorkflow
{
    public void Run()
    {
        var workingDirectory = cwd ?? Environment.CurrentDirectory;
        logger.LogDebug($"Working directory is '{workingDirectory}'");

        var cladConfig = new JsonObject
        {
            { "user", new JsonObject() },
            { "git", new JsonObject() }
        };
        var featureConfig = new Dictionary<string, object>
        {
            { "ghcr.io/mohrjonas/devcontainer-sudo/devcontainer-sudo:latest", new JsonObject() }
        };
        var defaultConfig = new DevContainerBuilder()
            .WithSchema("https://raw.githubusercontent.com/MohrJonas/IronClad/refs/heads/master/schema.json")
            .WithImage("debian:latest")
            .WithFeatures(featureConfig)
            .WithCustomizations(
                new DevContainerCustomizationsBuilder()
                .WithIronClad(cladConfig)
                .Build()
            )
            .Build();

        var cladFile = Path.Combine(workingDirectory, configPath ?? ".clad.json");
        logger.LogInformation($"Writing config to {cladFile}");

        if (File.Exists(cladFile))
        {
            var backupPath = Path.Combine(workingDirectory, $"{configPath ?? ".clad.json"}.bak");
            logger.LogInformation("Config already exists, backing it up first");
            File.Move(cladFile, backupPath);
        }

        File.WriteAllText(cladFile, JsonSerializer.Serialize(defaultConfig, SourceGenerationContext.Default.DevContainer));
    }
}
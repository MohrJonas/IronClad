using System.Text.Json;
using Mohr.Jonas.IronClad.Exceptions;
using Mohr.Jonas.IronClad.Features;
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class BuildConfigWorkflow(ILogger logger, string? cwd, string? cladConfigPath) : IWorkflow
{
    public void Run()
    {
        var workingDirectory = cwd ?? Environment.CurrentDirectory;
        logger.LogDebug($"Working directory is '{workingDirectory}'");

        if (cladConfigPath != null && !Path.IsPathRooted(cladConfigPath))
        {
            logger.LogDebug("Making config path absolute");
            cladConfigPath = Path.Combine(workingDirectory, cladConfigPath);
            logger.LogDebug($"Absolute config path is {cladConfigPath}");
        }

        var cladFile = IOUtils.FindCladFile(
            logger,
            workingDirectory,
            cladConfigPath != null ? [cladConfigPath] : []
        ) ?? throw new NoConfigurationFileFoundException();

        logger.LogDebug("Reading clad configuration");
        var devContainer = JsonSerializer.Deserialize(File.ReadAllText(cladFile), SourceGenerationContext.Default.DevContainer);
        var builder = new DevContainerBuilder(devContainer);

        foreach (var pair in devContainer?.Customizations?.IronClad ?? [])
        {
            var featureName = pair.Key;
            logger.LogDebug($"Feature name is '{featureName}'");

            var featureConfiguration = pair.Value!.AsObject();
            var feature = FeatureFactory.GetFeatureByName(featureName, featureConfiguration, workingDirectory);
            logger.LogInformation($"Applying feature '{featureName}'");

            feature.Apply(builder);
        }

        var devcontainerPath = Path.Combine(workingDirectory, ".devcontainer.json");
        logger.LogDebug($"Writing config to '{devcontainerPath}'");

        File.WriteAllText(devcontainerPath, JsonSerializer.Serialize(builder.Build(), SourceGenerationContext.Default.DevContainer));
    }
}
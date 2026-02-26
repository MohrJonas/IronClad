using System.Text.Json;
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class DownWorkflow(ILogger logger, string? cwd) : IWorkflow
{
    public void Run()
    {
        var workingDirectory = cwd ?? Environment.CurrentDirectory;
        logger.LogDebug($"Working directory is '{workingDirectory}'");

        var containers = GetDevcontainers();
        logger.LogDebug($"Devcontainers are '{string.Join(", ", containers.Select(p => $"{p.Key} -> {p.Value}"))}'");

        if (containers.TryGetValue(workingDirectory, out var containerName))
        {
            logger.LogInformation($"Removing container '{containerName}'");
            var output = ShellUtils.RunCommand("docker", ["rm", "-f", containerName]);

            logger.LogDebug($"Exitcode is '{output.ExitCode}'");
            logger.LogDebug($"Stdout is '{output.Stdout}'");
            logger.LogDebug($"Stderr is '{output.Stderr}'");
            output.EnsureSuccessfulExit();
            logger.LogInformation("Done removing container");
        }
        else
            logger.LogWarning("No container found to down");
    }

    private Dictionary<string, string> GetDevcontainers()
    {
        var output = ShellUtils.RunCommand("docker", ["ps", "--all", "--format", "json", "--filter", "label=devcontainer.config_file"]);
        logger.LogDebug($"docker ps output is '{output}'");

        var lines = output.Stdout.Trim().Split(Environment.NewLine);
        logger.LogDebug($"docker ps output lines are '{string.Join(", ", lines)}'");

        return lines.Select(line =>
        {
            var jsonElement = JsonSerializer.Deserialize(line, SourceGenerationContext.Default.JsonObject)!;

            var labelsString = jsonElement["Labels"]!.ToString();
            logger.LogDebug($"Label string is {labelsString}");

            var labels = labelsString.Split(',');
            logger.LogDebug($"Labels are '{string.Join(",", labels)}'");

            var labelsDict = labels.Select(label =>
            {
                logger.LogDebug($"Label is '{label}'");

                var parts = label.Split('=');
                logger.LogDebug($"Label parts are '{string.Join(", ", parts)}'");

                if (parts.Length != 2)
                {
                    logger.LogDebug("Ignoring label, since it does not have two parts");
                    return null;
                }

                return new KeyValuePair<string, string>?(KeyValuePair.Create(parts[0], parts[1]));
            })
            .Where(l => l.HasValue)
            .Cast<KeyValuePair<string, string>>()
            .ToDictionary();

            var containerId = jsonElement["ID"]!.ToString();
            logger.LogDebug($"Container id is '{containerId}'");

            return KeyValuePair.Create(labelsDict["devcontainer.local_folder"], containerId);
        }).ToDictionary();
    }
}
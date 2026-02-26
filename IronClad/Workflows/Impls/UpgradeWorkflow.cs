using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class UpgradeWorkflow(ILogger logger, string? cwd, string? configPath) : IWorkflow
{
    public void Run()
    {
        new BuildConfigWorkflow(logger, cwd, configPath).Run();

        var workingDirectory = cwd ?? Environment.CurrentDirectory;
        logger.LogDebug($"Working directory is '{workingDirectory}'");

        logger.LogInformation("Invoking devcontainer binary");
        ShellUtils.RunCommand(
            "devcontainer",
            ["upgrade", "--config", ".devcontainer.json"],
            workingDirectory
        ).EnsureSuccessfulExit();
    }
}
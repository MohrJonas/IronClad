using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Text.Json;
using Mohr.Jonas.IronClad.Exceptions;
using Mohr.Jonas.IronClad.Features;
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Workflows.Impls;

public sealed class WatchWorkflow(ILogger logger, CancellationToken cancellationToken, string? cwd, string? cladConfigPath) : IWorkflow
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

        logger.LogDebug("Setting up file change listener");
        var watcher = new FileSystemWatcher(Directory.GetParent(cladFile)!.FullName)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
            Filter = Path.GetFileName(cladFile)
        };
        watcher.Changed += (_, e) =>
        {
            logger.LogDebug("Change detected, rebuilding config");
            new BuildConfigWorkflow(logger, cwd, cladConfigPath).Run();
        };
        watcher.EnableRaisingEvents = true;
        cancellationToken.WaitHandle.WaitOne();
    }
}
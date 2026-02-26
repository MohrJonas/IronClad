using System.CommandLine;
using Mohr.Jonas.IronClad.Commands;
using Mohr.Jonas.IronClad.Commands.Impls;
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad;

internal static class Program
{
    private static int Main(string[] args)
    {
        var logger = new ConsoleLogger(LogLevel.Off);

        var root = new RootCommand();
        root.Options.Add(BaseArguments.Cwd);
        root.Options.Add(BaseArguments.ConfigPath);
        root.Options.Add(BaseArguments.ApplicationLogLevel);

        root.Subcommands.Add(new UpCommand(logger));
        root.Subcommands.Add(new DownCommand(logger));
        root.Subcommands.Add(new ExecCommand(logger));
        root.Subcommands.Add(new RunCommand(logger));
        root.Subcommands.Add(new UpgradeCommand(logger));

        var parseResult = root.Parse(args);
        logger.LogLevel = parseResult.CommandResult.GetRequiredValue<LogLevel>("--log-level");

        return parseResult.Invoke();
    }
}
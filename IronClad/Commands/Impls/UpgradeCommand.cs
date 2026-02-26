using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class UpgradeCommand : Command
{
    private readonly ILogger logger;

    public UpgradeCommand(ILogger logger) : base("upgrade", "Upgrade the lockfile")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Upgrading container lockfile", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);
    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var workflow = new UpgradeWorkflow(
                logger,
                parseResult.GetValue(BaseArguments.Cwd),
                parseResult.GetValue(BaseArguments.ConfigPath)
            );
        workflow.Run();
    }
}
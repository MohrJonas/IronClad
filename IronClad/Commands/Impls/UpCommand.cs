using System.CommandLine;
using System.Drawing;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class UpCommand : Command
{
    private readonly ILogger logger;

    public UpCommand(ILogger logger) : base("up", "Create a new container")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Setting up devcontainer", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);
    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var workflow = new UpWorkflow(
                logger,
                parseResult.GetValue(BaseArguments.Cwd),
                parseResult.GetValue(BaseArguments.ConfigPath)
            );
        workflow.Run();
    }
}

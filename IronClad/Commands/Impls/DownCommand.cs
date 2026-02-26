using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class DownCommand : Command
{
    private readonly ILogger logger;

    public DownCommand(ILogger logger) : base("down", "Stop and remove a container")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Removing container", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);
    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var workflow = new DownWorkflow(logger, parseResult.GetValue(BaseArguments.Cwd));
        workflow.Run();
    }
}
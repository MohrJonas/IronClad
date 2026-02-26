using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class RunCommand : Command
{
    private readonly ILogger logger;

    public RunCommand(ILogger logger) : base("run", "Run a command in a container")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Executing command", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);

    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var workflow = new RunWorkflow(
                logger,
                parseResult.GetValue(BaseArguments.Cwd),
                parseResult.GetValue(BaseArguments.ConfigPath),
                [.. parseResult.UnmatchedTokens]
            );
        workflow.Run();
    }
}
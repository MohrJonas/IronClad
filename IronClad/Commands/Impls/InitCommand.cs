using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class InitCommand : Command
{
    private readonly ILogger logger;

    public InitCommand(ILogger logger) : base("init", "Create a default config")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Creating config", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);
    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var workflow = new InitWorkflow(
                logger,
                parseResult.GetValue(BaseArguments.Cwd),
                parseResult.GetValue(BaseArguments.ConfigPath)
            );
        workflow.Run();
    }
}
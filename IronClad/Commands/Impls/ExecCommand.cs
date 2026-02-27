using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class ExecCommand : Command
{
    private readonly ILogger logger;

    public ExecCommand(ILogger logger) : base("exec", "Run a command in the container interactively")
    {
        this.logger = logger;
        SetAction(Execute);
        TreatUnmatchedTokensAsErrors = false;
    }

    public void Execute(ParseResult parseResult)
    {
        var workflow = new ExecWorkflow(
            logger,
            parseResult.GetValue(BaseArguments.Cwd),
            parseResult.GetValue(BaseArguments.ConfigPath),
            [.. parseResult.UnmatchedTokens]
        );
        workflow.Run();
    }
}
using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class BuildCommand : Command
{
    private readonly ILogger logger;

    public BuildCommand(ILogger logger) : base("build", "Generate a devcontainer config with features applied")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Building config", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);
    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var workflow = new BuildConfigWorkflow(
            logger, 
            parseResult.GetValue(BaseArguments.Cwd), 
            parseResult.GetValue(BaseArguments.ConfigPath)            
        );
        workflow.Run();
    }
}
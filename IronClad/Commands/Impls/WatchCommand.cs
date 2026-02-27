using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;
using Mohr.Jonas.IronClad.Workflows.Impls;

namespace Mohr.Jonas.IronClad.Commands.Impls;

internal sealed class WatchCommand : Command
{
    private readonly ILogger logger;

    public WatchCommand(ILogger logger) : base("watch", "Watch config and rebuild on change")
    {
        this.logger = logger;
        SetAction(Execute);
    }

    public void Execute(ParseResult parseResult)
    {
        if (logger.LogLevel == LogLevel.Off)
            Spinner.SpinToCompletion("Watching for config changes. Press Ctrl-C to stop", () => ExecuteInternal(parseResult));
        else
            ExecuteInternal(parseResult);
    }

    private void ExecuteInternal(ParseResult parseResult)
    {
        var tokenSource = new CancellationTokenSource();
        logger.LogDebug("Installing ctrl-c listener");
        Console.CancelKeyPress += (_, _) 
            => tokenSource.Cancel();
        var workflow = new WatchWorkflow(
            logger, 
            tokenSource.Token,
            parseResult.GetValue(BaseArguments.Cwd), 
            parseResult.GetValue(BaseArguments.ConfigPath)            
        );
        workflow.Run();
    }
}
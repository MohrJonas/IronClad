using System.CommandLine;
using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad.Commands;

internal static class BaseArguments
{
    public static Option<string> Cwd = new("--cwd")
    {
        Description = "The working directory to operate in"
    };

    public static Option<string> ConfigPath = new("--config-path")
    {
        Description = "The path of the config file to use, relative to the working directory"
    };

    public static Option<LogLevel> ApplicationLogLevel = new("--log-level")
    {
        Description = "Log-level of the application",
        DefaultValueFactory = (_) => LogLevel.Off
    };

    static BaseArguments()
    {
        Cwd.Validators.Add(value =>
        {
            if (!Directory.Exists(value.GetValue<string>("--cwd")))
                value.AddError("Working directory does not exist");
        });
    }
}
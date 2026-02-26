namespace Mohr.Jonas.IronClad.Logging;

public sealed class ConsoleLogger(LogLevel logLevel) : ILogger
{
    public LogLevel LogLevel { get; set; } = logLevel;

    public void Log(LogLevel level, string message)
    {
        if (level >= LogLevel)
            Console.WriteLine($"[{GetColorForLevel(level)}{GetPrefixStringForLevel(level)}\u001b[0m] {message}");
    }

    private static string GetColorForLevel(LogLevel level) => level switch
    {
        LogLevel.Debug => AsAnsiSequence(8),
        LogLevel.Information => AsAnsiSequence(251),
        LogLevel.Warning => AsAnsiSequence(208),
        LogLevel.Error => AsAnsiSequence(196),
        _ => throw new NotSupportedException()
    };

    private static string AsAnsiSequence(int color) => $"\u001b[38;5;{color}m";

    private static string GetPrefixStringForLevel(LogLevel level) => level switch
    {
        LogLevel.Debug => "Debug",
        LogLevel.Information => " Info",
        LogLevel.Warning => " Warn",
        LogLevel.Error => "Error",
        _ => throw new NotSupportedException()
    };
}
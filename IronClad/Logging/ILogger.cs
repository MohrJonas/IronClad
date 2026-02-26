namespace Mohr.Jonas.IronClad.Logging;

public interface ILogger
{
    public LogLevel LogLevel { get; set; }

    public void Log(LogLevel logLevel, string message);
}
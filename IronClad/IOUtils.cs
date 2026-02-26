using Mohr.Jonas.IronClad.Logging;

namespace Mohr.Jonas.IronClad;

public static class IOUtils
{
    public static string? FindCladFile(ILogger logger, string workingDirectory, params string[] additionalSearchPaths)
    {
        string[] potentialFilePaths = [
                Path.Combine(workingDirectory, ".clad.json"),
                Path.Combine(workingDirectory, ".ironclad", "clad.json"),
                ..additionalSearchPaths
            ];
        foreach (var potentialFilePath in potentialFilePaths)
        {
            logger.LogDebug($"Checking potential path {potentialFilePath}");
            if (File.Exists(potentialFilePath))
            {
                logger.LogInformation($"Found clad file at {potentialFilePath}");
                return potentialFilePath;
            }
            else
                logger.LogDebug($"Path {potentialFilePath} does not exist");
        }
        logger.LogWarning("Could not find clad file");
        return null;
    }

    public static string ResolvePathRelativeTo(string path, string directory) => path.Replace(directory, string.Empty);
}
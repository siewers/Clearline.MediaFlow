namespace Clearline.MediaFlow;

internal static class PathResolver
{
    public static FileInfo ResolvePath(string environmentVariableName)
    {
        // Read the path from environment variable
        var pathFromEnvironment = Environment.GetEnvironmentVariable(environmentVariableName);

        if (!string.IsNullOrWhiteSpace(pathFromEnvironment) && File.Exists(pathFromEnvironment))
        {
            return new FileInfo(pathFromEnvironment);
        }

        throw new InvalidOperationException($"Path for {environmentVariableName} is not set or does not exist.");

        // TODO: Add support for searching the path for any FF* executables
    }
}

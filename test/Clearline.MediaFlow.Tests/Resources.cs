namespace Clearline.MediaFlow.Tests;

using System.Reflection;

public static class Resources
{
    public static readonly DirectoryInfo ResourcesDirectory
        = new(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", Assembly.GetExecutingAssembly().GetName().Name!, "Resources")));

    public static FileInfo GetResourceFilePath(string fileName)
    {
        var filePath = Path.Combine(ResourcesDirectory.FullName, fileName);

        return File.Exists(filePath)
            ? new FileInfo(filePath)
            : throw new FileNotFoundException($"Resource file '{fileName}' not found in '{ResourcesDirectory.FullName}'.");
    }
}

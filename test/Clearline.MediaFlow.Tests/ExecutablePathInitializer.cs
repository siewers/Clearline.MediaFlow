namespace Clearline.MediaFlow.Tests;

using OperatingSystem = System.OperatingSystem;

public sealed class ExecutablePathInitializer : IAsyncLifetime
{
    private const string FFmpegVersion = "7.1.1";

    public ValueTask InitializeAsync()
    {
        if (OperatingSystem.IsWindows())
        {
            var basePath = Path.Combine(Resources.ResourcesDirectory.FullName, "win-x64");

            var ffmpegPath = new FileInfo(Path.Combine(basePath, $"ffmpeg-{FFmpegVersion}.exe"));
            FFmpegPath.Value = ffmpegPath;

            var ffprobePath = new FileInfo(Path.Combine(basePath, $"ffprobe-{FFmpegVersion}.exe"));
            FFprobePath.Value = ffprobePath;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}

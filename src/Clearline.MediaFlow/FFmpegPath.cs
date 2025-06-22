namespace Clearline.MediaFlow;

public static class FFmpegPath
{
    private static FileInfo? _customPath;
    private static readonly Lazy<FileInfo> LazyPath = new(() => PathResolver.ResolvePath("FFMPEG_PATH"));

    public static FileInfo Value
    {
        get => _customPath ?? LazyPath.Value;
        set => _customPath = ValidateCustomPath(value);
    }

    private static FileInfo ValidateCustomPath(FileInfo customPath)
    {
        return customPath.Exists
            ? customPath
            : throw new ArgumentException("Custom path must point to a valid FFmpeg executable file.");
    }
}

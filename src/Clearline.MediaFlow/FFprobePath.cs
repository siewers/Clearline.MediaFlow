namespace Clearline.MediaFlow;

public static class FFprobePath
{
    private static FileInfo? _customPath;
    private static readonly Lazy<FileInfo> LazyPath = new(() => PathResolver.ResolvePath("FFPROBE_PATH"));

    public static FileInfo Value
    {
        get => _customPath ?? LazyPath.Value;
        set => _customPath = ValidateCustomPath(value);
    }

    private static FileInfo ValidateCustomPath(FileInfo customPath)
    {
        return customPath.Exists
            ? customPath
            : throw new ArgumentException("Custom path must point to a valid FFprobe executable file.");
    }
}

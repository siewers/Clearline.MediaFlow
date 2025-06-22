namespace Clearline.MediaFlow;

/// <summary>
///     Default Implementation of the IInputBuilder Interface
/// </summary>
[PublicAPI]
public sealed class InputBuilder : IInputBuilder
{
    /// <inheritdoc />
    public List<FileInfo> FileList { get; } = [];

    /// <inheritdoc />
    public Func<string, string> PrepareInputFiles(IEnumerable<MediaLocation> files, out string directory)
    {
        var filesArray = files.ToArray();
        var directoryGuid = Guid.NewGuid();
        var directoryPath = directory = Path.Combine(Path.GetTempPath(), directoryGuid.ToString());

        Directory.CreateDirectory(directoryPath);

        for (var i = 0; i < filesArray.Length; i++)
        {
            var destinationPath = Path.Combine(directoryPath, BuildFileName(i + 1, Path.GetExtension(filesArray[i])));
            File.Copy(filesArray[i], destinationPath);
            FileList.Add(new FileInfo(destinationPath));
        }

        return index => $" -i {Path.Combine(directoryPath, $"img{index}{FileList[0].Extension}").Escape()}";
    }

    private static string BuildFileName(int fileIndex, string extension)
    {
        var name = "img_";

        name += fileIndex switch
        {
            < 10 => $"00{fileIndex}" + extension,
            < 100 => $"0{fileIndex}" + extension,
            _ => $"{fileIndex}" + extension,
        };

        return name;
    }
}

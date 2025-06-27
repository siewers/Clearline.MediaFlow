namespace Clearline.MediaFlow.Tests;

public class InputBuilderTests
{
    [Fact]
    public void PrepareInputFilesTest()
    {
        var files = Directory.EnumerateFiles(MediaFiles.Images)
                             .Select(MediaLocation.Create);

        var builder = new InputBuilder();

        builder.PrepareInputFiles(files, out var directory);
        var preparedFiles = Directory.EnumerateFiles(directory).ToList();

        builder.FileList.Should().HaveCount(12);
        preparedFiles.Should().HaveCount(builder.FileList.Count);
    }
}

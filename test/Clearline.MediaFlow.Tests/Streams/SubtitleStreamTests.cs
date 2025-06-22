namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class SubtitleStreamTests(StorageFixture storageFixture)
    : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData(Format.ass, FileExtension.Ass, "ass")]
    [InlineData(Format.webvtt, FileExtension.Vtt, "webvtt")]
    [InlineData(Format.srt, FileExtension.Srt, "subrip")]
    public async Task ConvertTest(Format format, FileExtension extension, string expectedFormat)
    {
        var outputPath = storageFixture.CreateFile().WithExtension(extension);

        var info = await MediaInfo.GetMediaInfoAsync(MediaFiles.SubtitleSrt, _testCancellationToken);

        var subtitleStream = info.SubtitleStreams.FirstOrDefault();
        await FFmpeg.Conversions.Create()
                    .AddStream(subtitleStream)
                    .SetOutput(outputPath)
                    .SetOutputFormat(format)
                    .Start(_testCancellationToken);

        var resultInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        Assert.Single(resultInfo.SubtitleStreams);
        var resultSteam = resultInfo.SubtitleStreams.First();
        resultSteam.Codec.Should().Be(expectedFormat);
    }

    [Theory]
    [InlineData(FileExtension.Ass, "ass")]
    [InlineData(FileExtension.Vtt, "webvtt")]
    [InlineData(FileExtension.Srt, "subrip")]
    public async Task ExtractSubtitles(FileExtension extension, string expectedFormat)
    {
        var outputPath = storageFixture.CreateFile().WithExtension(extension);
        var info = await MediaInfo.GetMediaInfoAsync(MediaFiles.MultipleStream, _testCancellationToken);

        var subtitleStream = info.SubtitleStreams.Single(x => x.Language == "spa");

        await FFmpeg.Conversions.Create()
                    .AddStream(subtitleStream)
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var resultInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);

        // Assert
        using (new AssertionScope())
        {
            resultInfo.VideoStreams.Should().BeEmpty();
            resultInfo.AudioStreams.Should().BeEmpty();
            resultInfo.SubtitleStreams.Should().ContainSingle().Subject
                      .Should().Satisfy<ISubtitleStream>(stream =>
                                                         {
                                                             stream.Codec.Should().Be(expectedFormat);
                                                             stream.IsDefault.Should().BeFalse();
                                                             stream.IsForced.Should().BeFalse();
                                                         }
                                                        );
        }
    }
}

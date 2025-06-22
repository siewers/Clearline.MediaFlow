namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class ConversionToFormatTests(StorageFixture storageFixture)
    : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    public static IEnumerable<object[]> JoinFiles =>
    [
        [MediaFiles.MkvWithAudio, MediaFiles.Mp4WithAudio, 23, 1280, 720, "16:9"],
        [MediaFiles.MkvWithAudio, MediaFiles.MkvWithAudio, 19, 320, 240, "4:3"],
        [MediaFiles.MkvWithAudio, MediaFiles.Mp4, 23, 1280, 720, "16:9"],
    ];

    [Theory]
    [InlineData(1, 0, 25)]
    [InlineData(1, 1, 24.889)]
    public async Task ToGifTest(int loopCount, int delay, double framerate)
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Gif);

        // Act
        _ = await (await FFmpeg.Conversions.FromSnippet.ToGif(MediaFiles.Mp4, output, loopCount, delay, _testCancellationToken))
                  .SetPreset(ConversionPreset.UltraFast)
                  .Start(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        using (new AssertionScope())
        {
            mediaInfo.Duration.Should().BeCloseTo(13.Seconds().And(480.Milliseconds()), 100.Milliseconds());
            mediaInfo.AudioStreams.Should().BeEmpty();
            mediaInfo.VideoStreams.Should().ContainSingle()
                     .Which.Should().Satisfy<IVideoStream>(videoStream =>
                                                           {
                                                               videoStream.Codec.Should().Be(VideoCodec.gif);
                                                               videoStream.Ratio.Should().Be("16:9");
                                                               videoStream.Framerate.Should().Be(framerate);
                                                               videoStream.Width.Should().Be(1280);
                                                               videoStream.Height.Should().Be(720);
                                                           }
                                                          );
        }
    }

    [Fact]
    public async Task ToMp4Test()
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        // Act
        await FFmpeg.Conversions.FromSnippet.ToMp4(MediaFiles.MkvWithAudio, output, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        using (new AssertionScope())
        {
            mediaInfo.Duration.Should().Be(9.Seconds().And(880.Milliseconds()));
            mediaInfo.AudioStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be("aac");

            mediaInfo.VideoStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be(VideoCodec.h264);
        }
    }

    [Fact]
    public async Task ToOgvTest()
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Ogv);

        // Act
        await FFmpeg.Conversions.FromSnippet.ToOgv(MediaFiles.MkvWithAudio, output, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        using (new AssertionScope())
        {
            mediaInfo.Duration.Should().Be(9.Seconds().And(880.Milliseconds()));
            mediaInfo.AudioStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be("vorbis");

            mediaInfo.VideoStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be(VideoCodec.Theora);
        }
    }

    [Fact]
    public async Task ToTsTest()
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Ts);

        // Act
        await FFmpeg.Conversions.FromSnippet.ToTs(MediaFiles.Mp4WithAudio, output, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        using (new AssertionScope())
        {
            mediaInfo.Duration.Should().Be(13.Seconds().And(512.Milliseconds()));
            mediaInfo.AudioStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be("mp2");

            mediaInfo.VideoStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be(VideoCodec.Mpeg2Video);
        }
    }

    [Fact]
    public async Task ToWebMTest()
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.WebM);

        // Act
        _ = await (await FFmpeg.Conversions.FromSnippet.ToWebM(MediaFiles.Mp4WithAudio, output, _testCancellationToken))
                  .SetPreset(ConversionPreset.UltraFast)
                  .Start(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        using (new AssertionScope())
        {
            mediaInfo.Duration.Should().Be(13.Seconds().And(507.Milliseconds()));
            mediaInfo.AudioStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be("vorbis");

            mediaInfo.VideoStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be(VideoCodec.vp8);
        }
    }

    [Fact]
    public async Task ConversionWithoutSpecificFormat()
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        // Act
        await Conversion.ConvertAsync(MediaFiles.MkvWithAudio, output, cancellationToken: _testCancellationToken)
                        .StartConversion(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        using (new AssertionScope())
        {
            mediaInfo.Duration.Should().Be(9.Seconds().And(880.Milliseconds()));
            mediaInfo.AudioStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be("aac");

            mediaInfo.VideoStreams.Should().ContainSingle()
                     .Which.Codec.Should().Be(VideoCodec.h264);
        }
    }
}

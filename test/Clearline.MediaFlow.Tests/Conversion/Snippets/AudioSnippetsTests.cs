namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class AudioSnippetsTests(StorageFixture storageFixture)
    : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task AddAudio()
    {
        // Arrange
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .AddAudio(MediaFiles.Mp4, MediaFiles.Mp3, output, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        // Act
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        // Assert
        mediaInfo.AudioStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IAudioStream>(stream =>
                                                       {
                                                           stream.Codec.Should().Be("aac");
                                                           stream.Duration.Should().Be(13.Seconds().And(504.Milliseconds()));
                                                       }
                                                      );
    }

    [Fact]
    public async Task ExtractAudio()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);
        await FFmpeg.Conversions.FromSnippet
                    .ExtractAudio(MediaFiles.Mp4WithAudio, output, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        mediaInfo.VideoStreams.Should().BeEmpty();
        mediaInfo.AudioStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IAudioStream>(stream =>
                                                       {
                                                           stream.Codec.Should().Be("mp3");
                                                           stream.Duration.Should().Be(13.Seconds().And(536.Milliseconds()));
                                                           stream.Bitrate.Should().Be(320000);
                                                       }
                                                      );
    }

    [Theory]
    [CombinatorialData]
    public async Task VisualiseAudioTest(VisualisationMode mode, AmplitudeScale amplitudeScale, FrequencyScale frequencyScale)
    {
        // Arrange
        const VideoSize size = VideoSize.Hd1080;
        const PixelFormat pixelFormat = PixelFormat.yuv420p;
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var originalMediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        await FFmpeg.Conversions.FromSnippet
                    .VisualizeAudio(MediaFiles.Mp4WithAudio, output, size, pixelFormat, mode, amplitudeScale, frequencyScale, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        // Act
        var resultFile = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        // Assert
        // The resulting streams are 4 seconds longer than the original
        var expectedDuration = originalMediaInfo.Duration.Add(4.Seconds());
        var precision = 500.Milliseconds();
        resultFile.VideoStreams.Should().ContainSingle()
                  .Which.Should().Satisfy<IVideoStream>(videoStream =>
                                                        {
                                                            videoStream.Duration.Should().BeCloseTo(expectedDuration, precision);
                                                            videoStream.Width.Should().Be(1920);
                                                            videoStream.Height.Should().Be(1080);
                                                        }
                                                       );

        resultFile.AudioStreams.Should().ContainSingle()
                  .Which.Duration.Should().BeCloseTo(expectedDuration, precision);
    }
}

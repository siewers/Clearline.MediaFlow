namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class MediaInfoRtspTests(MediaMtxServerFixture rtspServer) : IClassFixture<MediaMtxServerFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetMediaInfo_RTSP_CorrectDataIsShown()
    {
        var resourceUri = await rtspServer.Publish(MediaFiles.BunnyMp4, "bunny2");

        var result = await MediaInfo.GetMediaInfoAsync(resourceUri, _testCancellationToken);

        // Assert
        using (new AssertionScope())
        {
            result.SubtitleStreams.Should().BeEmpty();
            result.VideoStreams.Should().ContainSingle()
                  .Which.Should().Satisfy<IVideoStream>(videoStream =>
                                                        {
                                                            videoStream.Codec.Should().Be(VideoCodec.h264);
                                                            videoStream.Framerate.Should().Be(23.976);
                                                            videoStream.Width.Should().Be(640);
                                                            videoStream.Height.Should().Be(360);
                                                        }
                                                       );

            result.AudioStreams.Should().ContainSingle()
                  .Which.Should().Satisfy<IAudioStream>(audioStream => { audioStream.Codec.Should().Be("aac"); }
                                                       );
        }
    }

    [Fact]
    public async Task GetMediaInfo_NotExistingRtspServer_ThrowException()
    {
        var exception = await Record.ExceptionAsync(async () => await MediaInfo.GetMediaInfoAsync("rtsp://xabe.net/notExisting", _testCancellationToken));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public async Task RTSP_NotExistingStream_CanceledAfter30Seconds()
    {
        // Arrange
        var invalidRtspUrl = MediaLocation.Create(new Uri("rtsp://192.168.1.123:554"));

        // Act
        var act = () => MediaInfo.GetMediaInfoAsync(invalidRtspUrl, _testCancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task RTSP_NotExistingStream_CanceledAfter2Seconds()
    {
        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_testCancellationToken, new CancellationTokenSource(2.Seconds()).Token);

        var exception = await Record.ExceptionAsync(async () => await MediaInfo.GetMediaInfoAsync("rtsp://192.168.1.123:554/", cancellationTokenSource.Token));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public async Task GetMediaInfo_StreamDoesNotExist_ThrowException()
    {
        var exception = await Record.ExceptionAsync(async () => await MediaInfo.GetMediaInfoAsync("rtsp://127.0.0.1:8554/notExisting", _testCancellationToken));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }
}

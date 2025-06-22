namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class ConversionRtspTests(MediaMtxServerFixture mediaMtxServerFixture) : IClassFixture<MediaMtxServerFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task SendToRtspServer_MinimumConfiguration_FileIsBeingStreamed()
    {
        // Arrange
        var output = mediaMtxServerFixture.GetResourceUri("newFile");

        // Act
        var conversion = await FFmpeg.Conversions.FromSnippet.SendToRtspServer(MediaFiles.Mp4, output, _testCancellationToken);
        // Don't await since the task will never complete as it is a server stream
        _ = conversion.Start(_testCancellationToken);

        await Task.Delay(2.Seconds(), _testCancellationToken);

        // Assert
        var info = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        info.Streams.Should().ContainSingle();
    }

    [RunnableInDebugOnly]
    public async Task SendDesktopToRtspServer_MinimumConfiguration_DesktopIsBeingStreamed()
    {
        // Arrange
        var output = mediaMtxServerFixture.GetResourceUri("desktop");

        // Act
        _ = FFmpeg.Conversions.FromSnippet
                  .SendDesktopToRtspServer(output)
                  .Start(_testCancellationToken);

        //Give it some time to warm up
        await Task.Delay(2.Seconds(), _testCancellationToken);

        // Assert
        var info = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        info.Streams.Should().ContainSingle();
    }
}

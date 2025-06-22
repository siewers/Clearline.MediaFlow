namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class HardwareAcceleration(StorageFixture storageFixture)
    : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [RunnableInDebugOnly]
    public async Task ConversionWithHardware()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .ConvertWithHardwareAcceleration(MediaFiles.MkvWithAudio, output, HardwareAccelerator.auto, VideoCodec.h264_cuvid, VideoCodec.h264_nvenc, cancellationToken: _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.InRange(mediaInfo.Duration, 9.Seconds(), 11.Seconds());
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        Assert.NotNull(videoStream);
        Assert.NotNull(audioStream);
        Assert.Equal(VideoCodec.h264, videoStream.Codec);
        Assert.Equal("aac", audioStream.Codec);
    }
}

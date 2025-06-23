﻿namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class VideoSnippetsTests(StorageFixture storageFixture, MediaMtxServerFixture rtspServer)
    : IClassFixture<StorageFixture>, IClassFixture<MediaMtxServerFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    public static TheoryData<string, string, int, int, int, string> JoinFiles => new()
                                                                                 {
                                                                                     { MediaFiles.MkvWithAudio, MediaFiles.Mp4WithAudio, 23, 1280, 720, "16:9" },
                                                                                     { MediaFiles.MkvWithAudio, MediaFiles.MkvWithAudio, 19, 320, 240, "4:3" },
                                                                                     { MediaFiles.MkvWithAudio, MediaFiles.Mp4, 23, 1280, 720, "16:9" },
                                                                                 };

    [Theory]
    [MemberData(nameof(JoinFiles))]
    public async Task Concatenate_Test(string firstFile, string secondFile, int duration, int width, int height, string ratio)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var result = await FFmpeg.Conversions.FromSnippet.Concatenate(output, [firstFile, secondFile], _testCancellationToken).StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(duration, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        var videoStream = mediaInfo.VideoStreams.First();
        Assert.NotNull(videoStream);
        Assert.Equal(width, videoStream.Width);
        Assert.Equal(height, videoStream.Height);
        Assert.Contains($"-aspect {ratio}", result.Arguments);
    }

    [Fact]
    public async Task ChangeSizeTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var input = MediaFiles.MkvWithAudio;
        await FFmpeg.Conversions.FromSnippet.ChangeSize(input, output, width: 640, height: 360, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Single(mediaInfo.VideoStreams);
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        Assert.Equal(expected: 640, videoStream.Width);
        Assert.Equal(expected: 360, videoStream.Height);
    }

    [Fact]
    public async Task ExtractVideo()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet.ExtractVideo(MediaFiles.Mp4WithAudio, output, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Single(mediaInfo.VideoStreams);
        mediaInfo.AudioStreams.Should().BeEmpty();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(VideoCodec.h264);
    }

    [Fact]
    public async Task SnapshotInvalidArgumentTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Png);
        await Assert.ThrowsAsync<ArgumentException>(() => FFmpeg.Conversions.FromSnippet
                                                                .Snapshot(MediaFiles.Mp4WithAudio, output, 999.Seconds(), _testCancellationToken)
                                                                .StartConversion(_testCancellationToken)
                                                   );
    }

    [Theory]
    [InlineData(FileExtension.Png, 1825653)]
    [InlineData(FileExtension.Jpg, 84461)]
    public async Task SnapshotTest(FileExtension extension, long expectedLength)
    {
        var output = storageFixture.CreateFile().WithExtension(extension);
        await FFmpeg.Conversions.FromSnippet.Snapshot(MediaFiles.Mp4WithAudio, output, 0.Seconds(), _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        FileInfo fileInfo = (MediaLocation)output;
        fileInfo.Exists.Should().BeTrue();
        // It does not have to be the same
        Assert.Equal(expectedLength / 10, (await fileInfo.ReadAllBytesAsync(_testCancellationToken)).LongLength / 10);
    }

    [Fact]
    public async Task SplitVideoTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Split(MediaFiles.Mp4WithAudio, output, 2.Seconds(), 8.Seconds(), _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        audioStream.Codec.Should().Be("aac");
        videoStream.Codec.Should().Be(VideoCodec.h264);
        Assert.Equal(TimeSpan.FromSeconds(8), audioStream.Duration);
        Assert.Equal(TimeSpan.FromSeconds(8), videoStream.Duration);
        Assert.Equal(TimeSpan.FromSeconds(8), mediaInfo.Duration);
    }

    [Fact]
    public async Task WatermarkTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var result = await FFmpeg.Conversions.FromSnippet
                                 .SetWatermark(MediaFiles.Mp4WithAudio, output, MediaFiles.PngSample, Position.Center, _testCancellationToken)
                                 .StartConversion(_testCancellationToken);

        result.Arguments.Should().Contain("overlay=");
        result.Arguments.Should().Contain(MediaFiles.Mp4WithAudio);
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        audioStream.Codec.Should().Be("aac");
        videoStream.Codec.Should().Be(VideoCodec.h264);
    }

    [Fact]
    public async Task SaveM3U8Stream_Https_EverythingWorks()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var uri = new Uri("https://bitdash-a.akamaihd.net/content/MI201109210084_1/m3u8s/f08e80da-bf1d-4e3d-8899-f0f6155f6efa.m3u8");

        var exception = await Record.ExceptionAsync(() => FFmpeg.Conversions.FromSnippet
                                                                .SaveM3U8Stream(uri, output, 1.Seconds(), _testCancellationToken)
                                                                .StartConversion(_testCancellationToken)
                                                   );

        exception.Should().BeNull();
    }

    [Fact]
    public async Task SaveM3U8Stream_Http_EverythingWorks()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var uri = new Uri("https://bitdash-a.akamaihd.net/content/MI201109210084_1/m3u8s/f08e80da-bf1d-4e3d-8899-f0f6155f6efa.m3u8");

        var exception = await Record.ExceptionAsync(() => FFmpeg.Conversions.FromSnippet
                                                                .SaveM3U8Stream(uri, output, 1.Seconds(), _testCancellationToken)
                                                                .StartConversion(_testCancellationToken)
                                                   );

        exception.Should().BeNull();
    }

    [Fact]
    public async Task SaveM3U8Stream_NotExisting_ExceptionIsThrown()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var uri = new Uri("https://www.bitdash-a.akamaihd.net/notexisting.m3u8");

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.FromSnippet
                                                                            .SaveM3U8Stream(uri, output, 1.Seconds(), _testCancellationToken)
                                                                            .StartConversion(_testCancellationToken)
                                                   );

        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task BasicConversion_InputFileWithSubtitles_SkipSubtitles()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Convert(MediaFiles.MkvWithSubtitles, output, cancellationToken: _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(VideoCodec.h264);
        audioStream.Codec.Should().Be("aac");
        mediaInfo.SubtitleStreams.Should().BeEmpty();
        Assert.Equal(expected: 25, videoStream.Framerate);
    }

    [Fact]
    public async Task BasicConversion_InputFileWithSubtitles_SkipSubtitlesWithParameter()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Convert(MediaFiles.MkvWithSubtitles, output, keepSubtitles: false, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(VideoCodec.h264);
        audioStream.Codec.Should().Be("aac");
        mediaInfo.SubtitleStreams.Should().BeEmpty();
        Assert.Equal(expected: 25, videoStream.Framerate);
    }

    [Fact]
    public async Task BasicConversion_InputFileWithSubtitles_KeepSubtitles()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Convert(MediaFiles.MkvWithSubtitles, output, keepSubtitles: true, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        Assert.Equal(expected: 2, mediaInfo.SubtitleStreams.Count());
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(VideoCodec.h264);
        audioStream.Codec.Should().Be("aac");
        Assert.Equal(expected: 25, videoStream.Framerate);
    }

    public static TheoryData<VideoCodec, AudioCodec, SubtitleCodec> VideoAudioSubtitleCombinations => new()
                                                                                                      {
                                                                                                          { VideoCodec.h264, AudioCodec.Aac, SubtitleCodec.mov_text },
                                                                                                          { VideoCodec.hevc, AudioCodec.Aac, SubtitleCodec.mov_text },
                                                                                                      };

    [Theory]
    [MemberData(nameof(VideoAudioSubtitleCombinations))]
    public async Task BasicTranscode_InputFileWithSubtitles_KeepSubtitles(VideoCodec videoCodec, AudioCodec audioCodec, SubtitleCodec subtitleCodec)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Transcode(MediaFiles.MkvWithSubtitles, output, videoCodec, audioCodec, subtitleCodec, keepSubtitles: true, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        Assert.Equal(expected: 2, mediaInfo.SubtitleStreams.Count());
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(videoCodec);
        audioStream.Codec.Should().Be(audioCodec);
        Assert.Equal(expected: 25, videoStream.Framerate);
    }

    public static TheoryData<VideoCodec, AudioCodec, SubtitleCodec> VideoAudioSubtitleCombinationsWithCopy => new()
                                                                                                              {
                                                                                                                  { VideoCodec.h264, AudioCodec.Aac, SubtitleCodec.copy },
                                                                                                                  { VideoCodec.hevc, AudioCodec.Aac, SubtitleCodec.copy },
                                                                                                              };

    [Theory]
    [MemberData(nameof(VideoAudioSubtitleCombinationsWithCopy))]
    public async Task BasicTranscode_InputFileWithSubtitles_SkipSubtitles(VideoCodec videoCodec, AudioCodec audioCodec, SubtitleCodec subtitleCodec)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Transcode(MediaFiles.MkvWithSubtitles, output, videoCodec, audioCodec, subtitleCodec, keepSubtitles: false, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        mediaInfo.SubtitleStreams.Should().BeEmpty();
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(videoCodec);
        audioStream.Codec.Should().Be(audioCodec);
        Assert.Equal(expected: 25, videoStream.Framerate);
    }

    public static TheoryData<VideoCodec, AudioCodec, SubtitleCodec> WellKnownVideoCombinations => new()
                                                                                                  {
                                                                                                      { VideoCodec.h264, AudioCodec.Aac, SubtitleCodec.copy },
                                                                                                      { VideoCodec.hevc, AudioCodec.Aac, SubtitleCodec.copy },
                                                                                                  };

    [Theory]
    [MemberData(nameof(WellKnownVideoCombinations))]
    public async Task BasicTranscode_InputFileWithSubtitles_SkipSubtitlesWithParameter(VideoCodec videoCodec, AudioCodec audioCodec, SubtitleCodec subtitleCodec)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Transcode(MediaFiles.MkvWithSubtitles, output, videoCodec, audioCodec, subtitleCodec, keepSubtitles: false, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        mediaInfo.SubtitleStreams.Should().BeEmpty();
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        audioStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(videoCodec);
        audioStream.Codec.Should().Be(audioCodec);
        Assert.Equal(expected: 25, videoStream.Framerate);
    }

    [Fact]
    public async Task BasicConversion_SloMoVideo_CorrectFramerate()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Convert(MediaFiles.SloMoMp4, output, keepSubtitles: false, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 3, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        videoStream.Codec.Should().Be(VideoCodec.h264);
        mediaInfo.SubtitleStreams.Should().BeEmpty();
        // It does not have to be the same
        Assert.Equal(expected: 116, (int)videoStream.Framerate);
    }

    [Fact]
    public async Task BasicConversion_InputFileWithMultipleStreams_CorrectResult()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet
                    .Convert(MediaFiles.MultipleStream, output, keepSubtitles: false, _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 46, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Equal(expected: 2, mediaInfo.AudioStreams.Count());
        mediaInfo.SubtitleStreams.Should().BeEmpty();
        var videoStream = mediaInfo.VideoStreams.First();
        videoStream.Should().NotBeNull();
        Assert.Equal(expected: 24, videoStream.Framerate);
    }

    [Fact]
    public async Task Rtsp_GotTwoStreams_SaveEverything()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var resourceUri = await rtspServer.Publish(MediaFiles.BunnyMp4, "bunny");
        await Task.Delay(2.Seconds(), _testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(resourceUri, _testCancellationToken);

        await FFmpeg.Conversions.Create()
                    .AddStreams(mediaInfo.Streams)
                    .SetInputTime(TimeSpan.FromSeconds(3))
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var result = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        result.Duration.Should().BeGreaterThan(0.Seconds());
        Assert.Single(result.VideoStreams);
        Assert.Single(result.AudioStreams);
        result.SubtitleStreams.Should().BeEmpty();
        result.VideoStreams.First().Codec.Should().Be(VideoCodec.h264);
        Assert.Equal(expected: 23, (int)result.VideoStreams.First().Framerate);
        Assert.Equal(expected: 640, result.VideoStreams.First().Width);
        Assert.Equal(expected: 360, result.VideoStreams.First().Height);
        result.AudioStreams.First().Codec.Should().Be("aac");
    }
}

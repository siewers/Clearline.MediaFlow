namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class SubtitleSnippetsTests(StorageFixture storageFixture)
    : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task AddSubtitleTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var input = MediaFiles.MkvWithAudio;
        await FFmpeg.Conversions.FromSnippet.AddSubtitle(input, output, MediaFiles.SubtitleSrt, cancellationToken: _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var outputInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 51, outputInfo.Duration.Minutes);
        Assert.Equal(expected: 11, outputInfo.Duration.Seconds);
        Assert.Single(outputInfo.SubtitleStreams);
        Assert.Single(outputInfo.VideoStreams);
        Assert.Single(outputInfo.AudioStreams);
    }

    [Fact]
    public async Task AddSubtitleWithLanguageTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var input = MediaFiles.MkvWithAudio;

        var language = "pol";
        _ = await (await FFmpeg.Conversions.FromSnippet.AddSubtitle(input, output, MediaFiles.SubtitleSrt, language, _testCancellationToken))
                  .SetPreset(ConversionPreset.UltraFast)
                  .Start(_testCancellationToken);

        var outputInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 51, outputInfo.Duration.Minutes);
        Assert.Single(outputInfo.SubtitleStreams);
        Assert.Single(outputInfo.VideoStreams);
        Assert.Single(outputInfo.AudioStreams);
        Assert.Equal(language, outputInfo.SubtitleStreams.First().Language);
    }

    public static readonly TheoryData<SubtitleCodec> AddSubtitleTestCases = new(SubtitleCodec.webvtt, SubtitleCodec.subrip, SubtitleCodec.copy, SubtitleCodec.ass, SubtitleCodec.ssa
                                                                               );

    [Theory]
    [MemberData(nameof(AddSubtitleTestCases))]
    public async Task AddSubtitleWithCodecTest(SubtitleCodec subtitleCodec)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var input = MediaFiles.MkvWithAudio;
        await FFmpeg.Conversions.FromSnippet.AddSubtitle(input, output, MediaFiles.SubtitleSrt, subtitleCodec, cancellationToken: _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var outputInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 51, outputInfo.Duration.Minutes);
        Assert.Single(outputInfo.SubtitleStreams);
        Assert.Single(outputInfo.VideoStreams);
        Assert.Single(outputInfo.AudioStreams);

        if (subtitleCodec.ToString() == "copy")
        {
            Assert.Equal("subrip", outputInfo.SubtitleStreams.First().Codec);
        }
        else if (subtitleCodec.ToString() == "ssa")
        {
            Assert.Equal("ass", outputInfo.SubtitleStreams.First().Codec);
        }
        else
        {
            Assert.Equal(subtitleCodec.ToString(), outputInfo.SubtitleStreams.First().Codec);
        }
    }

    [Theory]
    [MemberData(nameof(AddSubtitleTestCases))]
    public async Task AddSubtitleWithLanguageAndCodecTest(SubtitleCodec subtitleCodec)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mkv);
        var input = MediaFiles.MkvWithAudio;

        var language = "pol";
        _ = await (await FFmpeg.Conversions.FromSnippet.AddSubtitle(input, output, MediaFiles.SubtitleSrt, subtitleCodec, language, _testCancellationToken))
                  .SetPreset(ConversionPreset.UltraFast)
                  .Start(_testCancellationToken);

        var outputInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 51, outputInfo.Duration.Minutes);
        Assert.Single(outputInfo.SubtitleStreams);
        Assert.Single(outputInfo.VideoStreams);
        Assert.Single(outputInfo.AudioStreams);
        Assert.Equal(language, outputInfo.SubtitleStreams.First().Language);

        if (subtitleCodec.ToString() == "copy")
        {
            Assert.Equal("subrip", outputInfo.SubtitleStreams.First().Codec);
        }
        else if (subtitleCodec.ToString() == "ssa")
        {
            Assert.Equal("ass", outputInfo.SubtitleStreams.First().Codec);
        }
        else
        {
            Assert.Equal(subtitleCodec.ToString(), outputInfo.SubtitleStreams.First().Codec);
        }
    }

    [Fact]
    public async Task BurnSubtitleTest()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var input = MediaFiles.Mp4;

        _ = await (await FFmpeg.Conversions.FromSnippet.BurnSubtitle(input, output, MediaFiles.SubtitleSrt, _testCancellationToken))
                  .SetPreset(ConversionPreset.UltraFast)
                  .Start(_testCancellationToken);

        var outputInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 13, outputInfo.Duration.Seconds);
    }

    [Fact]
    public async Task BasicConversion_InputFileWithSubtitles_SkipSubtitles()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.FromSnippet.Convert(MediaFiles.MkvWithSubtitles, output, cancellationToken: _testCancellationToken)
                    .StartConversion(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Single(mediaInfo.VideoStreams);
        Assert.Single(mediaInfo.AudioStreams);
        var audioStream = mediaInfo.AudioStreams.First();
        var videoStream = mediaInfo.VideoStreams.First();
        Assert.NotNull(videoStream);
        Assert.NotNull(audioStream);
        Assert.Equal(VideoCodec.h264, videoStream.Codec);
        Assert.Equal("aac", audioStream.Codec);
        Assert.Empty(mediaInfo.SubtitleStreams);
    }
}

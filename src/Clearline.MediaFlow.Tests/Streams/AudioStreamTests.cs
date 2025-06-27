﻿namespace Clearline.MediaFlow.Tests;

using System.Globalization;
using Exceptions;
using Fixtures;

public class AudioStreamTests(StorageFixture storageFixture) : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData(13, 13, 1.0)]
    [InlineData(6, 6, 2.0)]
    [InlineData(27, 27, 0.5)]
    public async Task ChangeSpeedTest(int expectedDuration, int expectedAudioDuration, double speed)
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.AudioStreams.First().ChangeSpeed(speed))
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        Assert.Equal(expectedDuration, mediaInfo.Duration.Seconds);
        Assert.Equal(expectedAudioDuration, mediaInfo.AudioStreams.First().Duration.Seconds);
        mediaInfo.AudioStreams.First().Codec.Should().Be(AudioCodec.Mp3);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Theory]
    [InlineData(192000)]
    [InlineData(32000)]
    public async Task SetBitrate(int expectedBitrate)
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetBitrate(expectedBitrate);
        await FFmpeg.Conversions.Create()
                    .AddStream(audioStream)
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);

        Assert.Equal(expectedBitrate, mediaInfo.AudioStreams.First().Bitrate);
        mediaInfo.AudioStreams.First().Codec.Should().Be(AudioCodec.Mp3);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Fact]
    public async Task SetBitrate_WithMaximumBitrate()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetBitrate(minBitrate: 32000, maxBitrate: 32000, bufferSize: 8000);
        await FFmpeg.Conversions.Create()
                    .AddStream(audioStream)
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);

        // Assert
        mediaInfo.AudioStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IAudioStream>(stream =>
                                                       {
                                                           stream.Bitrate.Should().Be(32000);
                                                           stream.Codec.Should().Be(AudioCodec.Mp3);
                                                       }
                                                      );
    }

    [Fact]
    public async Task ChangeChannels()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);

        var audioStream = inputFile.AudioStreams.First();
        var channels = audioStream.Channels;
        Assert.Equal(expected: 2, channels);
        audioStream.SetChannels(1);
        await FFmpeg.Conversions.Create()
                    .AddStream(audioStream)
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);

        Assert.Equal(expected: 1, mediaInfo.AudioStreams.First().Channels);
        mediaInfo.AudioStreams.First().Codec.Should().Be(AudioCodec.Mp3);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Fact]
    public async Task ChangeSamplerate()
    {
        // Arrange
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);

        var audioStream = inputFile.AudioStreams.Should().ContainSingle().Subject;
        audioStream.SampleRate.Should().Be(48000);

        // Act
        audioStream.SetSampleRate(44100);
        await FFmpeg.Conversions.Create()
                    .AddStream(audioStream)
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        // Assert
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        mediaInfo.AudioStreams.Should().ContainSingle().Which.Should().Satisfy<IAudioStream>(stream =>
                                                                                             {
                                                                                                 stream.SampleRate.Should().Be(44100);
                                                                                                 stream.Codec.Should().Be(AudioCodec.Mp3);
                                                                                             }
                                                                                            );
    }

    [Fact]
    public async Task OnConversion_ExtractOnlyAudioStream_OnProgressFires()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var conversion = FFmpeg.Conversions.Create()
                               .AddStream(inputFile.AudioStreams.First()
                                                   .SetSeek(TimeSpan.FromSeconds(2))
                                         )
                               .SetOutput(outputPath);

        var currentProgress = TimeSpan.Zero;
        var videoLength = TimeSpan.Zero;
        conversion.OnProgress += (_, e) =>
                                 {
                                     currentProgress = e.Duration;
                                     videoLength = e.TotalLength;
                                 };

        await conversion.Start(_testCancellationToken);

        currentProgress.Should().BeGreaterThan(TimeSpan.Zero);
        currentProgress.Should().BeLessThanOrEqualTo(videoLength);
        videoLength.TotalSeconds.Should().Be(7);
    }

    [Fact]
    public async Task ExtractAdditionalValuesTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        inputFile.AudioStreams.First().IsDefault.Should().BeTrue();
        inputFile.AudioStreams.First().IsForced.Should().BeFalse();
        inputFile.AudioStreams.First().Language.Should().Be("und");
    }

    [Fact]
    public async Task ChangeSpeed_CommaAsASeparator_CorrectResult()
    {
        CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("pl-PL");

        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.AudioStreams.First().ChangeSpeed(0.5))
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        Assert.Equal(expected: 27, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 27, mediaInfo.AudioStreams.First().Duration.Seconds);
        Assert.Equal("mp3", mediaInfo.AudioStreams.First().Codec);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Fact]
    public async Task SetBitstreamFilter_CorrectInput_CorrectResult()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.AudioStreams.First().SetBitstreamFilter(BitstreamFilter.aac_adtstoasc))
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 9, mediaInfo.AudioStreams.First().Duration.Seconds);
        Assert.Equal("aac", mediaInfo.AudioStreams.First().Codec);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Fact]
    public async Task SetBitstreamFilter_IncorrectFilter_ThrowConversionException()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.Create()
                                                                            .AddStream(inputFile.AudioStreams.First().SetBitstreamFilter(BitstreamFilter.h264_mp4toannexb))
                                                                            .SetOutput(outputPath)
                                                                            .Start(_testCancellationToken)
                                                   );

        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidBitstreamFilterException>();
    }

    [Fact]
    public async Task SetBitstreamFilter_CorrectInputAsString_CorrectResult()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.AudioStreams.First().SetBitstreamFilter("aac_adtstoasc"))
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 9, mediaInfo.AudioStreams.First().Duration.Seconds);
        Assert.Equal("aac", mediaInfo.AudioStreams.First().Codec);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Fact]
    public async Task SetBitstreamFilter_IncorrectFilterAsString_ThrowConversionException()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.Create()
                                                                            .AddStream(inputFile.AudioStreams.First().SetBitstreamFilter("h264_mp4toannexb"))
                                                                            .SetOutput(outputPath)
                                                                            .Start(_testCancellationToken)
                                                   );

        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidBitstreamFilterException>();
    }

    public static readonly TheoryData<AudioCodec, string> AudioCodecTestCases = new()
                                                                                {
                                                                                    { AudioCodec.Mp2, "mp2" },
                                                                                    { AudioCodec._4gv, "4gv" },
                                                                                    { AudioCodec._8svx_exp, "8svx_exp" },
                                                                                    { AudioCodec._8svx_fib, "8svx_fib" },
                                                                                    { AudioCodec.ComfortNoise, "comfortnoise" },
                                                                                    { AudioCodec.Copy, "copy" },
                                                                                };

    [Theory]
    [MemberData(nameof(AudioCodecTestCases))]
    public async Task ChangeCodec_EnumValue_EverythingMapsCorrectly(AudioCodec audioCodec, string expectedCodec)
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4WithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetCodec(audioCodec);

        var args = FFmpeg.Conversions.Create()
                         .AddStream(audioStream)
                         .SetOutput(outputPath)
                         .Build();

        Assert.Contains($"-c:a {expectedCodec}", args);
    }

    [Fact]
    public async Task ChangeCodec_StringValue_CorrectResult()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4WithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetCodec("mp3");
        await FFmpeg.Conversions.Create()
                    .AddStream(audioStream)
                    .SetOutput(outputPath)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);
        Assert.Equal(expected: 13, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 13, mediaInfo.AudioStreams.First().Duration.Seconds);
        Assert.Equal("mp3", mediaInfo.AudioStreams.First().Codec);
        Assert.NotEmpty(mediaInfo.AudioStreams);
    }

    [Fact]
    public async Task ChangeCodec_IncorrectCodec_NotFound()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4WithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetCodec("notExisting");

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.Create()
                                                                            .AddStream(audioStream)
                                                                            .SetOutput(outputPath)
                                                                            .Start(_testCancellationToken)
                                                   );

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ConversionException>();
    }

    [Fact]
    public async Task CopyStream_CorrectFFmpegArguments()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4WithAudio, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetCodec(AudioCodec.ComfortNoise);
        audioStream.CopyStream();

        var conversionResult = await FFmpeg.Conversions.Create()
                                           .AddStream(audioStream)
                                           .SetOutput(outputPath)
                                           .Start(_testCancellationToken);

        conversionResult.Arguments.Should().Be($"-i \"{Path.GetFullPath(inputFile.Location)}\" -c:a copy -map 0:1 -n \"{outputPath}\"");
    }

    [Fact]
    public async Task SetInputFormat_ChangeIfFormatIsApplied()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);
        var outputPath = storageFixture.CreateFile().WithExtension(FileExtension.Mp3);

        var audioStream = inputFile.AudioStreams.First();
        audioStream.SetInputFormat(Format.mp3);

        var result = await FFmpeg.Conversions.Create()
                                 .AddStream(audioStream)
                                 .SetOutput(outputPath)
                                 .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(outputPath, _testCancellationToken);

        result.Arguments.Should().Contain("-f mp3 -i");
        mediaInfo.AudioStreams.Should().NotBeEmpty();
    }
}

﻿namespace Clearline.MediaFlow.Tests;

using System.Globalization;
using Exceptions;
using Fixtures;

public class VideoStreamTests(StorageFixture storageFixture)
    : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData(RotateDegrees.Clockwise)]
    [InlineData(RotateDegrees.Invert)]
    public async Task TransposeTest(RotateDegrees rotateDegrees)
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First().Rotate(rotateDegrees))
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task Pad()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        videoStream.Pad(width: 480, height: 640);

        await FFmpeg.Conversions.Create()
                    .AddStream(videoStream)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 480, mediaInfo.VideoStreams.First().Width);
        Assert.Equal(expected: 640, mediaInfo.VideoStreams.First().Height);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task ChangeFramerate()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        var originalFramerate = videoStream.Framerate;
        Assert.Equal(expected: 25, originalFramerate);
        videoStream.SetFramerate(24);
        await FFmpeg.Conversions.Create()
                    .AddStream(videoStream)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 24, mediaInfo.VideoStreams.First().Framerate);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task SetBitrate()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        videoStream.Bitrate.Should().Be(860233);

        videoStream.SetBitrate(860237);
        await FFmpeg.Conversions.Create()
                    .AddStream(videoStream)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        mediaInfo.VideoStreams.First().Bitrate.Should().BeInRange(minimumValue: 560000, maximumValue: 580000);
        mediaInfo.VideoStreams.First().Codec.Should().Be(VideoCodec.h264);
        mediaInfo.AudioStreams.Should().BeEmpty();
    }

    [Fact]
    public async Task SetBitrate_WithMaxBitrateAndBuffer()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        var originalBitrate = videoStream.Bitrate;
        originalBitrate.Should().Be(860233);

        videoStream.SetBitrate(minBitrate: 6000, maxBitrate: 6000, bufferSize: 6000);
        await FFmpeg.Conversions.Create()
                    .AddStream(videoStream)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        mediaInfo.VideoStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IVideoStream>(stream =>
                                                       {
                                                           stream.Bitrate.Should().BeInRange(minimumValue: 7000, maximumValue: 8000);
                                                           stream.Codec.Should().Be(VideoCodec.h264);
                                                       }
                                                      );

        mediaInfo.AudioStreams.Should().BeEmpty();
    }

    // Check if Filter Flags do work. FFProbe does not support checking for Interlaced or Progressive,
    //  so there is no "real check" here
    [Fact]
    public async Task SetFlags()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        videoStream.SetFlags("ilme", "ildct");

        var result = await FFmpeg.Conversions.Create()
                                 .AddStream(videoStream)
                                 .SetOutput(output)
                                 .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Contains("-flags +ilme+ildct", result.Arguments);
    }

    // Check if Filter Flags do work. FFProbe does not support checking for Interlaced or Progressive,
    //  so there is no "real check" here
    [Fact]
    public async Task SetFlags_FlagsWithPlus_CorrectConversion()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        videoStream.SetFlags("ilme", "ildct");

        var result = await FFmpeg.Conversions.Create()
                                 .AddStream(videoStream)
                                 .SetOutput(output)
                                 .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Contains("-flags +ilme+ildct", result.Arguments);
    }

    // Check if Filter Flags do work. FFProbe does not support checking for Interlaced or Progressive,
    //  so there is no "real check" here
    [Fact]
    public async Task SetFlags_UseString_CorrectConversion()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        videoStream.SetFlags(Flag.ilme, Flag.ildct);

        var result = await FFmpeg.Conversions.Create()
                                 .AddStream(videoStream)
                                 .SetOutput(output)
                                 .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Contains("-flags +ilme+ildct", result.Arguments);
    }

    // Check if Filter Flags do work. FFProbe does not support checking for Interlaced or Progressive,
    //  so there is no "real check" here
    [Fact]
    public async Task SetFlags_ConcatenatedFlags_CorrectConversion()
    {
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var videoStream = inputFile.VideoStreams.First();
        videoStream.SetFlags("+ilme+ildct");

        var result = await FFmpeg.Conversions.Create()
                                 .AddStream(videoStream)
                                 .SetOutput(output)
                                 .Start(_testCancellationToken);

        result.Arguments.Should().Contain("-flags +ilme+ildct");

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        mediaInfo.VideoStreams.First().Codec.Should().Be(VideoCodec.h264);
        mediaInfo.AudioStreams.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1.0, 9)]
    [InlineData(2.0, 5)]
    [InlineData(0.5, 19)]
    public async Task ChangeSpeedTest(double speed, int expectedVideoDuration)
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        var result = await FFmpeg.Conversions.Create()
                                 .AddStream(inputFile.VideoStreams.First().SetCodec(VideoCodec.h264).ChangeSpeed(speed))
                                 .SetPreset(ConversionPreset.UltraFast)
                                 .SetOutput(output)
                                 .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        mediaInfo.AudioStreams.Should().BeEmpty();
        mediaInfo.VideoStreams.First().Should().Satisfy<IVideoStream>(stream =>
                                                                      {
                                                                          stream.Duration.Should().BeCloseTo(expectedVideoDuration.Seconds(), 1.Seconds());
                                                                          stream.Codec.Should().Be(VideoCodec.h264);
                                                                      }
                                                                     );
    }

    [Theory]
    [InlineData(0.4)]
    [InlineData(2.5)]
    public async Task ChangeMediaSpeedSTestArgumentOutOfRange(double multiplier)
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await FFmpeg.Conversions.Create()
                                                                                      .AddStream(inputFile.VideoStreams.First()
                                                                                                          .SetCodec(VideoCodec.h264)
                                                                                                          .ChangeSpeed(multiplier)
                                                                                                )
                                                                                      .SetPreset(ConversionPreset.UltraFast)
                                                                                      .SetOutput(output)
                                                                                      .Start(_testCancellationToken)
                                                             );
    }

    [Fact]
    public async Task BurnSubtitlesTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First().AddSubtitles(MediaFiles.SubtitleSrt))
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        mediaInfo.Duration.Should().Be(9.Seconds().And(840.Milliseconds()));
        mediaInfo.VideoStreams.First().Codec.Should().Be(VideoCodec.h264);
        mediaInfo.AudioStreams.Should().BeEmpty();
    }

    [Fact]
    public async Task BurnSubtitlesWithParametersTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var conversion = FFmpeg.Conversions.Create()
                               .AddStream(inputFile.VideoStreams.First()
                                                   .AddSubtitles(MediaFiles.SubtitleSrt, VideoSize.Xga, "UTF-8", "Fontsize=20,PrimaryColour=&H00ffff&,MarginV=30")
                                         )
                               .SetOutput(output);

        await conversion.Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Contains(":charenc=UTF-8:force_style='Fontsize=20,PrimaryColour=&H00ffff&,MarginV=30':original_size=1024x768", conversion.Build());
    }

    [Fact]
    public async Task ChangeOutputFramesCountTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetOutputFramesCount(50)
                              )
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(TimeSpan.FromSeconds(2), mediaInfo.Duration);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Equal(expected: 50, mediaInfo.Duration.TotalSeconds * mediaInfo.VideoStreams.First().Framerate);
    }

    [Fact]
    public async Task IncompatibleParametersTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var act = async () =>
                  {
                      await FFmpeg.Conversions.Create()
                                  .AddStream(inputFile.VideoStreams.First()
                                                      .SetCodec(VideoCodec.h264)
                                                      .Reverse()
                                                      .CopyStream()
                                            )
                                  .SetOutput(output)
                                  .Start(_testCancellationToken);
                  };

        (await act.Should().ThrowExactlyAsync<ConversionException>())
            .Which.Should().Satisfy<ConversionException>(ex =>
                                                         {
                                                             ex.Arguments.Should().ContainAll("-c:v copy", "-vf reverse");
                                                             ex.Message.Should().Contain("Filtergraph 'reverse' was specified, but codec copy was selected. Filtering and streamcopy cannot be used together.");
                                                         }
                                                        );
    }

    [Fact]
    public async Task LoopTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Gif);
        _ = await FFmpeg.Conversions.Create()
                        .AddStream(inputFile.VideoStreams.First()
                                            .SetLoop(1)
                                  )
                        .SetOutput(output)
                        .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        mediaInfo.Duration.Should().Be(13.Seconds().And(480.Milliseconds()));
        mediaInfo.AudioStreams.Should().BeEmpty();
        mediaInfo.VideoStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IVideoStream>(stream =>
                                                       {
                                                           stream.Codec.Should().Be(VideoCodec.gif);
                                                           stream.Ratio.Should().Be("16:9");
                                                           stream.Framerate.Should().Be(25);
                                                           stream.Width.Should().Be(1280);
                                                           stream.Height.Should().Be(720);
                                                       }
                                                      );
    }

    [Fact]
    public async Task ReverseTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetCodec(VideoCodec.h264)
                                        .Reverse()
                              )
                    .SetPreset(ConversionPreset.UltraFast)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task SeekLengthTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var conversion = FFmpeg.Conversions.Create()
                               .AddStream(inputFile.VideoStreams.First())
                               .SetOutput(output)
                               .SetSeek(TimeSpan.FromSeconds(2));

        var currentProgress = TimeSpan.Zero;
        var videoLength = TimeSpan.Zero;
        conversion.OnProgress += (_, e) =>
                                 {
                                     currentProgress = e.Duration;
                                     videoLength = e.TotalLength;
                                 };

        await conversion.Start(_testCancellationToken);

        Assert.True(currentProgress > TimeSpan.Zero);
        Assert.True(currentProgress <= videoLength);
        Assert.True(videoLength == TimeSpan.FromSeconds(7));
    }

    [Fact]
    public async Task SimpleConversionTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First())
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task X265Test()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetCodec(VideoCodec.hevc)
                              )
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.hevc, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task SizeTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetSize(width: 640, height: 480)
                              )
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Equal(expected: 640, mediaInfo.VideoStreams.First().Width);
        Assert.Equal(expected: 480, mediaInfo.VideoStreams.First().Height);
    }

    [Fact]
    public async Task SetSize_UseEnum_ResultsAreCorrect()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetSize(VideoSize.Sntsc)
                              )
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
        Assert.Equal(expected: 640, mediaInfo.VideoStreams.First().Width);
        Assert.Equal(expected: 480, mediaInfo.VideoStreams.First().Height);
    }

    [Fact]
    public async Task VideoCodecTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Ts);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetCodec(VideoCodec.Mpeg2Video)
                              )
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 9, mediaInfo.Duration.Seconds);
        Assert.Equal(VideoCodec.Mpeg2Video, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task ExtractAdditionalValuesTest()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        inputFile.VideoStreams.First().IsDefault.Should().BeTrue();
        inputFile.VideoStreams.First().IsForced.Should().BeFalse();
    }

    [Fact]
    public async Task ChangeSpeed_CommaAsASeparator_CorrectResult()
    {
        CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("pl-PL");

        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First().SetCodec(VideoCodec.h264).ChangeSpeed(0.5))
                    .SetPreset(ConversionPreset.UltraFast)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 19, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 19, mediaInfo.VideoStreams.First().Duration.Seconds);
        Assert.Equal(VideoCodec.h264, mediaInfo.VideoStreams.First().Codec);
        Assert.False(mediaInfo.AudioStreams.Any());
    }

    [Fact]
    public async Task SetBitstreamFilter_CorrectInput_CorrectResult()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First().SetBitstreamFilter(BitstreamFilter.h264_mp4toannexb))
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 13, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 13, mediaInfo.VideoStreams.First().Duration.Seconds);
        Assert.NotEmpty(mediaInfo.VideoStreams);
    }

    [Fact]
    public async Task SetBitstreamFilter_IncorrectFilter_ThrowConversionException()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.Create()
                                                                            .AddStream(inputFile.VideoStreams.First().SetBitstreamFilter(BitstreamFilter.aac_adtstoasc))
                                                                            .SetOutput(output)
                                                                            .Start(_testCancellationToken)
                                                   );

        exception.Should().BeOfType<InvalidBitstreamFilterException>();
    }

    [Fact]
    public async Task SetBitstreamFilter_CorrectInputAsString_CorrectResult()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First().SetBitstreamFilter("h264_mp4toannexb"))
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 13, mediaInfo.Duration.Seconds);
        Assert.Equal(expected: 13, mediaInfo.VideoStreams.First().Duration.Seconds);
        Assert.NotEmpty(mediaInfo.VideoStreams);
    }

    [Fact]
    public async Task SetBitstreamFilter_IncorrectFilterAsString_ThrowConversionException()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.Create()
                                                                            .AddStream(inputFile.VideoStreams.First().SetBitstreamFilter("aac_adtstoasc"))
                                                                            .SetOutput(output)
                                                                            .Start(_testCancellationToken)
                                                   );

        exception.Should().BeOfType<InvalidBitstreamFilterException>();
    }

    public static TheoryData<VideoCodec, string> SetCodecSpecialNamesData => new()
                                                                             {
                                                                                 { VideoCodec._4xm, "4xm" },
                                                                                 { VideoCodec._8bps, "8bps" },
                                                                                 { VideoCodec._012v, "012v" },
                                                                             };

    [Theory]
    [MemberData(nameof(SetCodecSpecialNamesData))]
    public async Task SetCodec_SpecialNames_EverythingIsCorrect(VideoCodec videoCodec, string expectedCodec)
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var args = FFmpeg.Conversions.Create()
                         .AddStream(inputFile.VideoStreams.First()
                                             .SetCodec(videoCodec)
                                   )
                         .SetOutput(output)
                         .Build();

        Assert.Contains($"-c:v {expectedCodec}", args);
    }

    [Fact]
    public async Task SetCodec_InvalidCodec_ThrowConversionException()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);

        var exception = await Record.ExceptionAsync(async () => await FFmpeg.Conversions.Create()
                                                                            .AddStream(inputFile.VideoStreams.First().SetCodec("notExisting"))
                                                                            .SetOutput(output)
                                                                            .Start(_testCancellationToken)
                                                   );

        Assert.NotNull(exception);
        Assert.IsType<ConversionException>(exception);
    }

    [Fact]
    public async Task SetSize_ParameterIsOverridden_NewValueIsSet()
    {
        var inputFile = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        var output = storageFixture.CreateFile().WithExtension(FileExtension.Mp4);
        await FFmpeg.Conversions.Create()
                    .AddStream(inputFile.VideoStreams.First()
                                        .SetSize(width: 1920, height: 1080)
                                        .SetSize(width: 640, height: 480)
                              )
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        Assert.Equal(expected: 640, mediaInfo.VideoStreams.First().Width);
        Assert.Equal(expected: 480, mediaInfo.VideoStreams.First().Height);
    }
}

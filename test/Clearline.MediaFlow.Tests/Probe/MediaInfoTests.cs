namespace Clearline.MediaFlow.Tests;

using Fixtures;

public class MediaInfoTests(StorageFixture storageFixture) : IClassFixture<StorageFixture>
{
    private readonly CancellationToken _testCancellationToken = TestContext.Current.CancellationToken;

    [Fact]
    public async Task AudioPropertiesTest()
    {
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp3, _testCancellationToken);

        FileInfo mediaFile = mediaInfo.Location;
        mediaFile.Exists.Should().BeTrue();
        mediaFile.Name.Should().Be("audio.mp3");

        using (new AssertionScope())
        {
            var audioStream = mediaInfo.AudioStreams.Should().ContainSingle().Subject;
            audioStream.Should().NotBeNull();
            audioStream.Codec.Should().Be(AudioCodec.Mp3);
            audioStream.Duration.Should().Be(13.Seconds().And(536.Milliseconds()));

            mediaInfo.VideoStreams.Should().BeEmpty();

            mediaInfo.Duration.Should().Be(audioStream.Duration);
            mediaInfo.Size.Should().Be(216916);
        }
    }

    [Fact]
    public async Task GetMultipleStreamsTest()
    {
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.MultipleStream, _testCancellationToken);

        mediaInfo.VideoStreams.Should().ContainSingle();
        mediaInfo.AudioStreams.Should().HaveCount(2);
        mediaInfo.SubtitleStreams.Should().HaveCount(8);
    }

    [Fact]
    public async Task GetVideoBitrateTest()
    {
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        mediaInfo.VideoStreams.Should().ContainSingle()
                 .Which.Bitrate.Should().Be(860233);
    }

    [Fact]
    public async Task IncorrectFormatTest()
    {
        await FluentActions.Awaiting(() => MediaInfo.GetMediaInfoAsync(MediaFiles.Dll, _testCancellationToken))
                           .Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Mp4PropertiesTest()
    {
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.BunnyMp4, _testCancellationToken);

        mediaInfo.Streams.Should().NotBeEmpty();
    }

    [Fact]
    public async Task MkvPropertiesTest()
    {
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var mediaFile = new FileInfo(mediaInfo.Location);
        mediaFile.Exists.Should().BeTrue();
        mediaFile.Name.Should().Be("SampleVideo_360x240_1mb.mkv");

        var expectedDuration = 9.Seconds().And(818.Milliseconds());
        mediaInfo.AudioStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IAudioStream>(audioStream =>
                                                       {
                                                           audioStream.Should().NotBeNull();
                                                           audioStream.Codec.Should().Be(AudioCodec.Aac);
                                                           audioStream.Index.Should().Be(1);
                                                           audioStream.Duration.Should().Be(expectedDuration);
                                                       }
                                                      );

        mediaInfo.VideoStreams.Should().ContainSingle()
                 .Which.Should().Satisfy<IVideoStream>(videoStream =>
                                                       {
                                                           videoStream.Should().NotBeNull();
                                                           videoStream.Codec.Should().Be(VideoCodec.h264);
                                                           videoStream.Index.Should().Be(0);
                                                           videoStream.Framerate.Should().Be(25);
                                                           videoStream.Height.Should().Be(240);
                                                           videoStream.Width.Should().Be(320);
                                                           videoStream.Ratio.Should().Be("4:3");
                                                           videoStream.Duration.Should().Be(expectedDuration);
                                                       }
                                                      );

        mediaInfo.Duration.Should().Be(expectedDuration);
        mediaInfo.Size.Should().Be(1055721);
    }

    [Fact]
    public async Task PropertiesTest()
    {
        var mediaInfo = await MediaInfo.GetMediaInfoAsync(MediaFiles.Mp4WithAudio, _testCancellationToken);

        FileInfo mediaFile = mediaInfo.Location;

        using (new AssertionScope())
        {
            var expectedSize = 2107842;

            mediaFile.Exists.Should().BeTrue();
            mediaFile.Name.Should().Be("input.mp4");
            mediaFile.Length.Should().Be(expectedSize);

            mediaInfo.Size.Should().Be(expectedSize);
            mediaInfo.Duration.Should().Be(13.Seconds().And(504.Milliseconds()));

            mediaInfo.AudioStreams.Should().ContainSingle().Which
                     .Should().Satisfy<IAudioStream>(stream =>
                                                     {
                                                         stream.Codec.Should().Be(AudioCodec.Aac);
                                                         stream.Duration.Should().Be(13.Seconds().And(504.Milliseconds()));
                                                     }
                                                    );

            mediaInfo.VideoStreams.Should().ContainSingle().Which
                     .Should().Satisfy<IVideoStream>(stream =>
                                                     {
                                                         stream.Codec.Should().Be(VideoCodec.h264);
                                                         stream.Framerate.Should().Be(25);
                                                         stream.Height.Should().Be(720);
                                                         stream.Width.Should().Be(1280);
                                                         stream.Ratio.Should().Be("16:9");
                                                         stream.Duration.Should().Be(13.Seconds().And(480.Milliseconds()));
                                                     }
                                                    );
        }
    }

    [Theory]
    [InlineData("檔")]
    [InlineData("אספירין")]
    [InlineData("एस्पिरि")]
    [InlineData("阿司匹林")]
    [InlineData("アセチルサリチル酸")]
    public async Task GetMediaInfo_NonUTF8CharactersInPath(string path)
    {
        var output = storageFixture.CreateFile().WithName(path).WithExtension(FileExtension.Mp4);
        File.Copy(MediaFiles.Mp4WithAudio, output, overwrite: true);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);

        mediaInfo.Should().NotBeNull();
    }

    [Fact]
    public async Task CalculateFramerate_SloMoVideo_CorrectFramerateIsReturned()
    {
        var info = await MediaInfo.GetMediaInfoAsync(MediaFiles.SloMoMp4, _testCancellationToken);
        var videoStream = info.VideoStreams.First();

        // It does not have to be the same
        Assert.Equal(expected: 116, (int)videoStream.Framerate);
        Assert.Equal(expected: 3, videoStream.Duration.Seconds);
    }

    [Fact]
    public async Task MediaInfo_SpecialCharactersInName_WorksCorrectly()
    {
        var output = storageFixture.CreateFile().WithName("Crime d'Amour").WithExtension(FileExtension.Mp4);
        var info = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);

        var conversionResult = await FFmpeg.Conversions.Create()
                                           .AddStream(info.VideoStreams.First())
                                           .AddParameter("-re", ArgumentPosition.PreInput)
                                           .SetOutput(output)
                                           .Start(_testCancellationToken);

        var outputMediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        outputMediaInfo.Streams.Should().NotBeNull();
        conversionResult.Arguments.Should().Contain("Crime d'Amour");
    }

    [Fact]
    public async Task MediaInfo_NameWithSpaces_WorksCorrectly()
    {
        var fileName = Guid.NewGuid().ToString("N").Replace(oldChar: '-', newChar: ' ');
        var output = storageFixture.CreateFile().WithName(fileName).WithExtension(FileExtension.Mp4);
        var info = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        await FFmpeg.Conversions.Create()
                    .AddStream(info.VideoStreams.First())
                    .AddParameter("-re", ArgumentPosition.PreInput)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var outputMediaInfo = await MediaInfo.GetMediaInfoAsync(output, _testCancellationToken);
        outputMediaInfo.Streams.Should().NotBeNull();
    }

    [Fact]
    public async Task MediaInfo_EscapedString_WorksCorrectly()
    {
        var fileName = Guid.NewGuid().ToString("N").Replace(oldChar: '-', newChar: ' ');
        var output = storageFixture.CreateFile().WithName(fileName).WithExtension(FileExtension.Mp4);
        var info = await MediaInfo.GetMediaInfoAsync(MediaFiles.MkvWithAudio, _testCancellationToken);
        await FFmpeg.Conversions.Create()
                    .AddStream(info.VideoStreams.First())
                    .AddParameter("-re", ArgumentPosition.PreInput)
                    .SetOutput(output)
                    .Start(_testCancellationToken);

        var outputMediaInfo = await MediaInfo.GetMediaInfoAsync($"\"{output}\"", _testCancellationToken);
        outputMediaInfo.Streams.Should().NotBeNull();
    }

    [Fact]
    public async Task MediaInfo_EscapedString_BasePathDoesNotChange()
    {
        const string fileName = "AMD is NOT Ripping Off Intel - WAN Show April 30, 2021_v1";
        MediaLocation inputLocation = storageFixture.CreateFile().WithName(fileName).WithExtension(FileExtension.Mp4);

        File.Copy(MediaFiles.BunnyMp4, inputLocation, overwrite: true);

        var mediaInfo = await MediaInfo.GetMediaInfoAsync(inputLocation, _testCancellationToken);

        mediaInfo.Location.Should().Be(inputLocation);
    }

    [Fact]
    public async Task MediaInfo_EscapedString_BasePathInStreamsDoesNotChange()
    {
        const string fileName = "AMD is NOT Ripping Off Intel - WAN Show April 30, 2021_v2";
        MediaLocation input = storageFixture.CreateFile().WithName(fileName).WithExtension(FileExtension.Mp4);
        File.Copy(MediaFiles.BunnyMp4, input, overwrite: true);

        var info = await MediaInfo.GetMediaInfoAsync(input, _testCancellationToken);

        info.VideoStreams.First().Path.Should().Be($"\"{input}\"");
    }
}

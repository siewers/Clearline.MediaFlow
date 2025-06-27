namespace Clearline.MediaFlow.Tests;

using System.Reflection;

public static class MediaFiles
{
    public static MediaLocation PngSample => GetResourceFilePath("watermark.png");

    public static MediaLocation Mp4WithAudio => GetResourceFilePath("input.mp4");

    public static MediaLocation Mp3 => GetResourceFilePath("audio.mp3");

    public static MediaLocation Mp4 => GetResourceFilePath("mute.mp4");

    public static MediaLocation MkvWithAudio => GetResourceFilePath("SampleVideo_360x240_1mb.mkv");

    public static MediaLocation MkvWithSubtitles => GetResourceFilePath("mkvWithSubtitles.mkv");

    public static MediaLocation MultipleStream => GetResourceFilePath("multipleStreamSample.mkv");

    public static MediaLocation TsWithAudio => GetResourceFilePath("sample.ts");

    public static MediaLocation FlvWithAudio => GetResourceFilePath("sample.flv");

    public static MediaLocation BunnyMp4 => GetResourceFilePath("bunny.mp4");

    public static MediaLocation SloMoMp4 => GetResourceFilePath("slomo.mp4");

    public static MediaLocation Dll => Assembly.GetExecutingAssembly().Location;

    public static MediaLocation Images => GetResourceFilePath("Images");

    public static MediaLocation SubtitleSrt => GetResourceFilePath("sampleSrt.srt");

    public static MediaLocation FFbinariesInfo => GetResourceFilePath("ffbinaries.json");

    private static MediaLocation GetResourceFilePath(string fileName)
    {
        return Resources.GetResourceFilePath(fileName);
    }
}

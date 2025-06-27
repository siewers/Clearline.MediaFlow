namespace Clearline.MediaFlow.Sample;

using NewApi;

public class ExampleUsages
{
    public async Task Sample()
    {
        // Get media information from source file
        var mediaInfo = await MediaInfo.GetMediaInfoAsync("path/to/source/file.mp4");

        // Create conversion options
        var mediaConverter = new MediaConverter();
        mediaConverter.AddStreams(mediaInfo.VideoStreams);
        mediaConverter.AddStreams(mediaInfo.AudioStreams);
        mediaConverter.AddStreams(mediaInfo.SubtitleStreams);
    }
}

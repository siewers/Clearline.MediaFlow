namespace Clearline.MediaFlow.NewApi;

public class MediaConverter : IMediaConverter
{
    public IMediaConverter AddStreams(IEnumerable<IVideoStream> videoStreams, Action<VideoStreamConversionOptions>? options = null)
    {
        foreach (var videoStream in videoStreams)
        {
            AddStream(videoStream, options);
        }

        return this;
    }

    public IMediaConverter AddStreams(IEnumerable<IAudioStream> audioStreams, Action<AudioStreamConversionOptions>? options = null)
    {
        foreach (var audioStream in audioStreams)
        {
            AddStream(audioStream, options);
        }

        return this;
    }

    public IMediaConverter AddStreams(IEnumerable<ISubtitleStream> subtitleStreams, Action<SubtitleStreamConversionOptions>? options = null)
    {
        foreach (var subtitleStream in subtitleStreams)
        {
            AddStream(subtitleStream, options);
        }

        return this;
    }

    public IMediaConverter AddStream(IVideoStream videoStream, Action<VideoStreamConversionOptions>? options = null)
    {
        var videoStreamOptions = new VideoStreamConversionOptions(videoStream);

        if (options is not null)
        {
            options(videoStreamOptions);
        }
        else
        {
            videoStreamOptions.CopyStream(streamType: 'v');
        }

        return this;
    }

    public IMediaConverter AddStream(IAudioStream audioStream, Action<AudioStreamConversionOptions>? options = null)
    {
        var audioStreamOptions = new AudioStreamConversionOptions(audioStream);

        if (options is not null)
        {
            options(audioStreamOptions);
        }
        else
        {
            audioStreamOptions.CopyStream(streamType: 'a');
        }

        return this;
    }

    public IMediaConverter AddStream(ISubtitleStream subtitleStream, Action<SubtitleStreamConversionOptions>? options = null)
    {
        var subtitleStreamOptions = new SubtitleStreamConversionOptions(subtitleStream);

        if (options is not null)
        {
            options(subtitleStreamOptions);
        }
        else
        {
            subtitleStreamOptions.CopyStream(streamType: 's');
        }

        return this;
    }
}

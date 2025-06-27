namespace Clearline.MediaFlow.NewApi;

public interface IMediaConverter
{
    IMediaConverter AddStreams(IEnumerable<IVideoStream> videoStreams, Action<VideoStreamConversionOptions>? options = null);

    IMediaConverter AddStreams(IEnumerable<IAudioStream> audioStreams, Action<AudioStreamConversionOptions>? options = null);

    IMediaConverter AddStreams(IEnumerable<ISubtitleStream> subtitleStreams, Action<SubtitleStreamConversionOptions>? options = null);

    IMediaConverter AddStream(IVideoStream videoStream, Action<VideoStreamConversionOptions>? options = null);

    IMediaConverter AddStream(IAudioStream audioStream, Action<AudioStreamConversionOptions>? options = null);

    IMediaConverter AddStream(ISubtitleStream subtitleStream, Action<SubtitleStreamConversionOptions>? options = null);
}

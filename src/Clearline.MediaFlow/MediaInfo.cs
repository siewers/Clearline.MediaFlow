namespace Clearline.MediaFlow;

using Probe;
using Probe.Models;

public sealed class MediaInfo : IMediaInfo
{
    private MediaInfo(MediaLocation location, ProbeModel probeModel)
    {
        Location = location;
        Size = probeModel.Format.Size;
        CreationTime = probeModel.Format.Tags.CreationTime?.UtcDateTime;
        VideoStreams = probeModel.Streams.OfType<VideoStreamModel>().Select(stream => new VideoStream(stream, probeModel.Format));
        AudioStreams = probeModel.Streams.OfType<AudioStreamModel>().Select(stream => new AudioStream(stream, probeModel.Format));
        SubtitleStreams = probeModel.Streams.OfType<SubtitleStreamModel>().Select(stream => new SubtitleStream(stream, probeModel.Format));
        Duration = CalculateDuration(probeModel);
    }

    public MediaLocation Location { get; }

    public DateTime? CreationTime { get; }

    public long Size { get; }

    public TimeSpan Duration { get; }

    public IEnumerable<IStream> Streams => [..VideoStreams, ..AudioStreams, ..SubtitleStreams];

    public IEnumerable<IVideoStream> VideoStreams { get; }

    public IEnumerable<IAudioStream> AudioStreams { get; }

    public IEnumerable<ISubtitleStream> SubtitleStreams { get; }

    public async static Task<IMediaInfo> GetMediaInfoAsync(MediaLocation mediaLocation, CancellationToken cancellationToken = default)
    {
        var probeModel = await FFprobe.GetProbeModel(mediaLocation, cancellationToken);
        return new MediaInfo(mediaLocation, probeModel);
    }

    private static TimeSpan CalculateDuration(ProbeModel probeModel)
    {
        var audioMax = probeModel.Streams.OfType<AudioStreamModel>().Max(stream => stream.Duration);
        var videoMax = probeModel.Streams.OfType<VideoStreamModel>().Max(stream => stream.Duration);

        return (audioMax > videoMax ? audioMax : videoMax) ?? probeModel.Format.Duration;
    }
}

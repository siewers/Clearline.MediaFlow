namespace Clearline.MediaFlow;

public partial class Conversion
{
    internal async static Task<IConversion> AddSubtitleAsync(MediaLocation source, ISubtitleStream subtitleStream, MediaLocation destination, string? language, CancellationToken cancellationToken)
    {
        subtitleStream.SetLanguage(language);
        return await AddSubtitleAsync(source, subtitleStream, destination, cancellationToken);
    }

    internal async static Task<IConversion> AddSubtitleAsync(MediaLocation source, ISubtitleStream subtitleStream, MediaLocation destination, CancellationToken cancellationToken)
    {
        var sourceMediaInfo = await MediaInfo.GetMediaInfoAsync(source, cancellationToken);
        return Create().AddStreams(sourceMediaInfo.Streams)
                       .AddStream(subtitleStream)
                       .SetOutput(destination);
    }

    internal async static Task<IConversion> BurnSubtitlesAsync(MediaLocation source, MediaLocation outputLocation, MediaLocation subtitlesLocation, CancellationToken cancellationToken)
    {
        var info = await MediaInfo.GetMediaInfoAsync(source, cancellationToken);

        var videoStream = info.VideoStreams.FirstOrDefault()
                              ?.AddSubtitles(subtitlesLocation);

        return Create().AddStream(videoStream)
                       .AddStream(info.AudioStreams.FirstOrDefault())
                       .SetOutput(outputLocation);
    }

    internal async static Task<IConversion> AddSubtitleAsync(MediaLocation inputLocation, MediaLocation outputLocation, MediaLocation subtitleLocation, string? language, CancellationToken cancellationToken)
    {
        var sourceMediaInfo = await MediaInfo.GetMediaInfoAsync(inputLocation, cancellationToken);
        var subtitleInfo = await MediaInfo.GetMediaInfoAsync(subtitleLocation, cancellationToken);

        var subtitleStream = subtitleInfo.SubtitleStreams.First()
                                         .SetLanguage(language)
                                         .CopyStream();

        return Create().AddStreams(sourceMediaInfo.Streams)
                       .AddStream(subtitleStream)
                       .SetOutput(outputLocation);
    }

    internal async static Task<IConversion> AddSubtitleAsync(MediaLocation inputLocation, MediaLocation outputLocation, MediaLocation subtitleLocation, SubtitleCodec subtitleCodec, string? language, CancellationToken cancellationToken)
    {
        var sourceMediaInfo = await MediaInfo.GetMediaInfoAsync(inputLocation, cancellationToken);
        var subtitleInfo = await MediaInfo.GetMediaInfoAsync(subtitleLocation, cancellationToken);

        var subtitleStream = subtitleInfo.SubtitleStreams.First()
                                         .SetLanguage(language)
                                         .SetCodec(subtitleCodec);

        return Create().AddStreams(sourceMediaInfo.Streams)
                       .AddStream(subtitleStream)
                       .SetOutput(outputLocation);
    }
}

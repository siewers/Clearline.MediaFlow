namespace Clearline.MediaFlow;

using System.Text;
using Probe.Models;

internal abstract class StreamBase<TStream>(string path, int index)
    : IStream<TStream> where TStream : IStream<TStream>
{
    internal StreamBase(StreamModelBase streamModel, FormatModel formatModel)
        : this(formatModel.FileName, streamModel.Index)
    {
        Title = formatModel.Tags.Title;
        Codec = streamModel.CodecName;
        Language = streamModel.Tags.Language ?? "und";
        Duration = streamModel.Duration ?? streamModel.Tags.Duration ?? formatModel.Duration;
        Bitrate = Math.Abs(streamModel.Bitrate ?? streamModel.Tags.Bitrate ?? formatModel.Bitrate ?? 0);
        IsForced = streamModel.Disposition.IsForced;
        IsDefault = streamModel.Disposition.IsDefault;
    }

    public ConversionArguments Arguments { get; } = [];

    public string Path { get; } = path;

    public string? Title { get; }

    public int Index { get; } = index;

    public Codec Codec { get; }

    public string? Language { get; }

    public TimeSpan Duration { get; }

    public long Bitrate { get; }

    public bool? IsForced { get; }

    public bool? IsDefault { get; }

    protected FilterCollection Filters { get; } = [];

    public void AppendArguments(StringBuilder builder, ArgumentPosition forPosition)
    {
        var values = Arguments.Where(argument => argument.Position == forPosition).Select(p => p.Value);
        builder.Append(string.Join(separator: ' ', values));
    }

    public virtual IEnumerable<MediaLocation> GetSource()
    {
        return [Path];
    }

    public abstract TStream CopyStream();

    public abstract TStream SetCodec(Codec name);

    public abstract IEnumerable<IFilterConfiguration> GetFilters();

    public abstract TStream SetLanguage(string? lang);

    protected void SetLanguage(char streamType, string? lang)
    {
        var language = !string.IsNullOrEmpty(lang) ? lang : Language;

        if (string.IsNullOrEmpty(language))
        {
            return;
        }

        Arguments.AddPostInput($"metadata:{streamType}:s:{Index}", $"language={language}");
    }
}

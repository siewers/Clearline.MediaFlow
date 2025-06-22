namespace Clearline.MediaFlow;

// ReSharper disable InconsistentNaming
public readonly partial record struct SubtitleCodec
{
    /// <summary>
    ///     Copy the subtitle stream without re-encoding.
    /// </summary>
    public static SubtitleCodec copy => new("copy");

    /// <summary>
    ///     Advanced SubStation Alpha subtitle.
    /// </summary>
    public static SubtitleCodec ass { get; } = new("ass");

    /// <summary>
    ///     DVB subtitle.
    /// </summary>
    public static SubtitleCodec dvb_subtitle { get; } = new("dvb_subtitle");

    /// <summary>
    ///     DVD subtitle.
    /// </summary>
    public static SubtitleCodec dvd_subtitle { get; } = new("dvd_subtitle");

    /// <summary>
    ///     HDMV PGS subtitle.
    /// </summary>
    public static SubtitleCodec hdmv_pgs_subtitle { get; } = new("hdmv_pgs_subtitle");

    /// <summary>
    ///     HDMV Text subtitle.
    /// </summary>
    public static SubtitleCodec hdmv_text_subtitle { get; } = new("hdmv_text_subtitle");

    /// <summary>
    ///     JACOsub subtitle.
    /// </summary>
    public static SubtitleCodec jacosub { get; } = new("jacosub");

    /// <summary>
    ///     MicroDVD subtitle.
    /// </summary>
    public static SubtitleCodec microdvd { get; } = new("microdvd");

    /// <summary>
    ///     MOV text subtitle.
    /// </summary>
    public static SubtitleCodec mov_text { get; } = new("mov_text");

    /// <summary>
    ///     MPL2 subtitle.
    /// </summary>
    public static SubtitleCodec mpl2 { get; } = new("mpl2");

    /// <summary>
    ///     PJS subtitle.
    /// </summary>
    public static SubtitleCodec pjs { get; } = new("pjs");

    /// <summary>
    ///     RealText subtitle.
    /// </summary>
    public static SubtitleCodec realtext { get; } = new("realtext");

    /// <summary>
    ///     SAMI subtitle.
    /// </summary>
    public static SubtitleCodec sami { get; } = new("sami");

    /// <summary>
    ///     SubRip subtitle.
    /// </summary>
    public static SubtitleCodec srt { get; } = new("srt");

    /// <summary>
    ///     SubStation Alpha subtitle.
    /// </summary>
    public static SubtitleCodec ssa { get; } = new("ssa");

    /// <summary>
    ///     EBU STL subtitle.
    /// </summary>
    public static SubtitleCodec stl { get; } = new("stl");

    /// <summary>
    ///     SubRip subtitle.
    /// </summary>
    public static SubtitleCodec subrip { get; } = new("subrip");

    /// <summary>
    ///     SubViewer subtitle.
    /// </summary>
    public static SubtitleCodec subviewer { get; } = new("subviewer");

    /// <summary>
    ///     SubViewer v1 subtitle.
    /// </summary>
    public static SubtitleCodec subviewer1 { get; } = new("subviewer1");

    /// <summary>
    ///     VPlayer subtitle.
    /// </summary>
    public static SubtitleCodec vplayer { get; } = new("vplayer");

    /// <summary>
    ///     WebVTT subtitle.
    /// </summary>
    public static SubtitleCodec webvtt { get; } = new("webvtt");
}

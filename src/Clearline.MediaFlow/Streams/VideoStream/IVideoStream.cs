﻿namespace Clearline.MediaFlow;

/// <summary>
///     Video Stream
/// </summary>
[PublicAPI]
public interface IVideoStream : IStream<IVideoStream>
{
    /// <summary>
    ///     Width
    /// </summary>
    int Width { get; }

    /// <summary>
    ///     Height
    /// </summary>
    int Height { get; }

    /// <summary>
    ///     Frame rate
    /// </summary>
    double Framerate { get; }

    /// <summary>
    ///     Screen ratio
    /// </summary>
    string? Ratio { get; }

    /// <summary>
    ///     Pixel Format
    /// </summary>
    string? PixelFormat { get; }

    /// <summary>
    ///     Rotation angle
    /// </summary>
    int? Rotation { get; }

    new VideoCodec Codec { get; }

    /// <summary>
    ///     Rotate video
    /// </summary>
    /// <param name="rotateDegrees">Rotate type</param>
    /// <returns>IVideoStream</returns>
    IVideoStream Rotate(RotateDegrees rotateDegrees);

    /// <summary>
    ///     Pad the video to a specific height and width with black banners.
    /// </summary>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <returns>IVideoStream</returns>
    IVideoStream Pad(int width, int height);

    /// <summary>
    ///     Change speed of video
    /// </summary>
    /// <param name="multiplier">Speed value. (0.5 - 2.0). To double the speed set this to 2.0</param>
    /// <returns>IVideoStream</returns>
    /// <exception cref="ArgumentOutOfRangeException">When speed isn't between 0.5 - 2.0.</exception>
    IVideoStream ChangeSpeed(double multiplier);

    /// <summary>
    ///     Add watermark to video
    /// </summary>
    /// <param name="imagePath">Watermark</param>
    /// <param name="position">Position of watermark</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetWatermark(MediaLocation imagePath, Position position);

    /// <summary>
    ///     Reverse the video
    /// </summary>
    /// <returns>IVideoStream</returns>
    IVideoStream Reverse();

    /// <summary>
    ///     Set the flags for conversion (-flags option)
    /// </summary>
    /// <param name="flags">Flags to use</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetFlags(params Flag[] flags);

    /// <summary>
    ///     Set the flags for conversion (-flags option)
    /// </summary>
    /// <param name="flags">Flags to use</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetFlags(params string[] flags);

    /// <summary>
    ///     Set the framerate of the video (-r option)
    /// </summary>
    /// <param name="framerate">The framerate in FPS</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetFramerate(double framerate);

    /// <summary>
    ///     Set the bitrate of the video (-b:v option)
    /// </summary>
    /// <param name="minBitrate">The minimum bitrate in bits</param>
    /// <param name="maxBitrate">The maximum bitrate in bits</param>
    /// <param name="bufferSize">The buffer size in bits</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize);

    /// <summary>
    ///     Set the bitrate of the video (-b:v option)
    /// </summary>
    /// <param name="bitrate">Bitrate in bits</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetBitrate(long bitrate);

    /// <summary>
    ///     Set size of video
    /// </summary>
    /// <param name="size">VideoSize</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetSize(VideoSize size);

    /// <summary>
    ///     Set size of video
    /// </summary>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetSize(int width, int height);

    /// <summary>
    ///     Set video codec
    /// </summary>
    /// <param name="codec">Video codec</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetCodec(VideoCodec codec);

    /// <summary>
    ///     Set filter
    /// </summary>
    /// <param name="filter">Filter</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetBitstreamFilter(BitstreamFilter filter);

    /// <summary>
    ///     Loop over the input stream.(-loop)
    /// </summary>
    /// <param name="count">Number of repeats</param>
    /// <param name="delay">Delay between repeats (in seconds)</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetLoop(int count, int delay = 0);

    /// <summary>
    ///     Set output frames count
    /// </summary>
    /// <param name="number">Number of frames</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetOutputFramesCount(int number);

    /// <summary>
    ///     Seeks in input file to position. (-ss argument)
    /// </summary>
    /// <param name="seek">Position</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetSeek(TimeSpan seek);

    /// <summary>
    ///     Burn subtitle into file
    /// </summary>
    /// <param name="subtitleLocation">Path to subtitle file in .srt format</param>
    /// <param name="characterEncoding">Set subtitles input character encoding. Only useful if not UTF-8.</param>
    /// <param name="style">
    ///     Override default style or script info parameters of the subtitles. It accepts a string containing
    ///     ASS style format KEY=VALUE couples separated by ","
    /// </param>
    /// <returns>IVideoStream</returns>
    IVideoStream AddSubtitles(MediaLocation subtitleLocation, string? characterEncoding = null, string? style = null);

    /// <summary>
    ///     Burn subtitle into file
    /// </summary>
    /// <param name="subtitleLocation">Path to subtitle file in .srt format</param>
    /// <param name="characterEncoding">Set subtitles input character encoding. Only useful if not UTF-8.</param>
    /// <param name="style">
    ///     Override default style or script info parameters of the subtitles. It accepts a string containing
    ///     ASS style format KEY=VALUE couples separated by ","
    /// </param>
    /// <param name="originalSize">
    ///     Specify the size of the original video, the video for which the ASS style was composed. This
    ///     is necessary to correctly scale the fonts if the aspect ratio has been changed.
    /// </param>
    /// <returns>IVideoStream</returns>
    IVideoStream AddSubtitles(MediaLocation subtitleLocation, VideoSize originalSize, string? characterEncoding = null, string? style = null);

    /// <summary>
    ///     Get part of video
    /// </summary>
    /// <param name="startTime">Start point</param>
    /// <param name="duration">Duration of new video</param>
    /// <returns>IVideoStream</returns>
    IVideoStream Split(TimeSpan startTime, TimeSpan duration);

    /// <summary>
    ///     Set filter
    /// </summary>
    /// <param name="filter">Filter</param>
    /// <returns>IVideoStream</returns>
    IVideoStream SetBitstreamFilter(string filter);

    /// <summary>
    ///     Sets the format for the input file using the -f option before the input file name
    /// </summary>
    /// <param name="inputFormat">The input format to set</param>
    /// <returns>IConversion object</returns>
    IVideoStream SetInputFormat(string inputFormat);

    /// <summary>
    ///     Sets the format for the input file using the -f option before the input file name
    /// </summary>
    /// <param name="inputFormat">The input format to set</param>
    /// <returns>IConversion object</returns>
    IVideoStream SetInputFormat(Format inputFormat);

    /// <summary>
    ///     "-re" parameter. Read input at native frame rate. Mainly used to simulate a grab device, or live input stream (e.g.
    ///     when reading from a file). Should not be used with actual grab devices or live input streams (where it can cause
    ///     packet loss). By default ffmpeg attempts to read the input(s) as fast as possible. This option will slow down the
    ///     reading of the input(s) to the native frame rate of the input(s). It is useful for real-time output (e.g. live
    ///     streaming).
    /// </summary>
    /// <param name="readInputAtNativeFrameRate">Read input at native frame rate. False set parameter to default value.</param>
    /// <returns>IConversion object</returns>
    IVideoStream UseNativeInputRead(bool readInputAtNativeFrameRate);

    /// <summary>
    ///     "-stream_loop" parameter. Set number of times input stream shall be looped.
    /// </summary>
    /// <param name="loopCount">Loop 0 means no loop, loop -1 means infinite loop.</param>
    /// <returns>IConversion object</returns>
    IVideoStream SetStreamLoop(int loopCount);
}

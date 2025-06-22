namespace Clearline.MediaFlow.Exceptions;

internal sealed class FFmpegExceptionCatcher
{
    private static readonly List<ExceptionCheck> Checks = [];

    static FFmpegExceptionCatcher()
    {
        Checks.Add(new ExceptionCheck("Invalid NAL unit size", containsFileIsEmptyMessage: false, GenericConversionException.Create));
        Checks.Add(new ExceptionCheck("Packet mismatch", containsFileIsEmptyMessage: true, GenericConversionException.Create));
        Checks.Add(new ExceptionCheck("asf_read_pts failed", containsFileIsEmptyMessage: true, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("Missing key frame while searching for timestamp", containsFileIsEmptyMessage: true, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("Old interlaced mode is not supported", containsFileIsEmptyMessage: true, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("mpeg1video", containsFileIsEmptyMessage: true, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("Frame rate very high for a muxer not efficiently supporting it", containsFileIsEmptyMessage: true, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("multiple fourcc not supported", containsFileIsEmptyMessage: false, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("Unknown decoder", containsFileIsEmptyMessage: false, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("Failed to open codec in avformat_find_stream_info", containsFileIsEmptyMessage: false, UnknownDecoderException.Create));
        Checks.Add(new ExceptionCheck("Unrecognized hwaccel: ", containsFileIsEmptyMessage: false, HardwareAcceleratorNotFoundException.Create));
        Checks.Add(new ExceptionCheck("Unable to find a suitable output format", containsFileIsEmptyMessage: false, FFmpegNoSuitableOutputFormatFoundException.Create));
        Checks.Add(new ExceptionCheck("is not supported by the bitstream filter", containsFileIsEmptyMessage: false, InvalidBitstreamFilterException.Create));
    }

    internal static void CatchFFmpegErrors(string output, string args)
    {
        var firstError = Checks.FirstOrDefault(check => check.CheckLog(output));
        firstError?.Throw(output, args);
    }
}

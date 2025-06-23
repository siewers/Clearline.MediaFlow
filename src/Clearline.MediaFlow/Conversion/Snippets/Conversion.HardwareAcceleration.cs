﻿namespace Clearline.MediaFlow;

public partial class Conversion
{
    /// <summary>
    ///     Convert one file to another with destination format using hardware acceleration (if possible). Using cuvid. Works
    ///     only on Windows/Linux with NVidia GPU.
    /// </summary>
    /// <param name="inputLocation">Path to file</param>
    /// <param name="outputLocation">Path to file</param>
    /// <param name="hardwareAccelerator">
    ///     Hardware accelerator. List of all accelerators available for your system - "ffmpeg
    ///     -hwaccels"
    /// </param>
    /// <param name="decoder">Codec using to decoding input video (e.g. h264_cuvid)</param>
    /// <param name="encoder">Codec using to encode output video (e.g. h264_nvenc)</param>
    /// <param name="device">Number of device (0 = default video card) if more than one video card.</param>
    /// <returns>IConversion object</returns>
    internal async static Task<IConversion> ConvertWithHardwareAcceleration(MediaLocation inputLocation, MediaLocation outputLocation, HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
    {
        var conversion = await ConvertAsync(inputLocation, outputLocation);
        return conversion.UseHardwareAcceleration(hardwareAccelerator, decoder, encoder, device);
    }
}

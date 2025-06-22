namespace Clearline.MediaFlow;

/// <summary>
///     Wrapper for FFmpeg
/// </summary>
public abstract class FFmpeg
{
    /// <summary>
    ///     Get new instance of Conversion
    /// </summary>
    /// <returns>IConversion object</returns>
    public static Conversions Conversions { get; } = new();

    /// <summary>
    ///     Get available audio and video devices (like cams or mics)
    /// </summary>
    /// <returns>List of available devices</returns>
    internal async static Task<Device[]> GetAvailableDevices(CancellationToken cancellationToken = default)
    {
        return await Conversion.GetAvailableDevices(cancellationToken);
    }
}

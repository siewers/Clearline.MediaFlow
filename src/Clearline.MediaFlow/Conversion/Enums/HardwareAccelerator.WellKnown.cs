namespace Clearline.MediaFlow;

// ReSharper disable InconsistentNaming
public readonly partial record struct HardwareAccelerator
{
    /// <summary>
    ///     Automatically select the hardware acceleration method.
    /// </summary>
    public static HardwareAccelerator auto { get; } = new("auto");

    /// <summary>
    ///     Direct3D 11 Video Acceleration
    /// </summary>
    public static HardwareAccelerator d3d11va { get; } = new("d3d11va");

    /// <summary>
    ///     DirectX Video Acceleration
    /// </summary>
    public static HardwareAccelerator dxva2 { get; } = new("dxva2");

    /// <summary>
    ///     Intel QuickSync Video
    /// </summary>
    public static HardwareAccelerator qsv { get; } = new("qsv");

    /// <summary>
    ///     NVIDIA CUDA Video Decoder
    /// </summary>
    public static HardwareAccelerator cuvid { get; } = new("cuvid");

    /// <summary>
    ///     Video Decode and Presentation API for Unix
    /// </summary>
    public static HardwareAccelerator vdpau { get; } = new("vdpau");

    /// <summary>
    ///     Video Acceleration API
    /// </summary>
    public static HardwareAccelerator vaapi { get; } = new("vaapi");

    /// <summary>
    ///     Intel Media SDK
    /// </summary>
    public static HardwareAccelerator libmfx { get; } = new("libmfx");

    /// <summary>
    ///     Advanced Micro Devices Video Coding Engine
    /// </summary>
    public static HardwareAccelerator amf { get; } = new("amf");

    /// <summary>
    ///     Apple Video Toolbox (h264)
    /// </summary>
    public static HardwareAccelerator h264_videotoolbox { get; } = new("h264_videotoolbox");

    /// <summary>
    ///     Apple Video Toolbox (hevc)
    /// </summary>
    public static HardwareAccelerator hevc_videotoolbox { get; } = new("hevc_videotoolbox");

    /// <summary>
    ///     OpenCL Video Acceleration
    /// </summary>
    public static HardwareAccelerator opencl { get; } = new("opencl");

    /// <summary>
    ///     Vulkan Video Acceleration
    /// </summary>
    public static HardwareAccelerator vulkan { get; } = new("vulkan");

    /// <summary>
    ///     Direct3D 12 Video Acceleration
    /// </summary>
    public static HardwareAccelerator d3d12va { get; } = new("d3d12va");
}

namespace Clearline.MediaFlow.Tests;

using Exceptions;

public class FFmpegExceptionCatcherTests
{
    [Fact]
    public void CatchErrors_UnrecognizedHwacceel_ThrowHardwareAcceleratorNotFoundException()
    {
        //Arrange
        const string args = "args";
        var output = $"Unrecognized hwaccel: {Guid.NewGuid():N}. Supported hwaccels: cuda dxva2 qsv d3d11va qsv cuvid";

        //Act
        var exception = Record.Exception(() => FFmpegExceptionCatcher.CatchFFmpegErrors(output, args));

        //Assert
        exception.Should().Satisfy<HardwareAcceleratorNotFoundException>(Assertion);
        return;

        void Assertion(HardwareAcceleratorNotFoundException ex)
        {
            ex.Arguments.Should().Be(args);
            ex.Message.Should().Be(output);
        }
    }

    [Fact]
    public void CatchErrors_NoFFmpegError_NoExceptionIsThrown()
    {
        //Arrange
        const string args = "args";
        const string output = "FFmpeg result without exception";

        //Act
        var exception = Record.Exception(() => FFmpegExceptionCatcher.CatchFFmpegErrors(output, args));

        //Assert
        exception.Should().BeNull();
    }

    [Fact]
    public void CatchErrors_NoSuitableOutputFormat_ThrowFFmpegNoSuitableOutputFormatFoundException()
    {
        //Arrange
        const string args = "args";
        var output = $"Unable to find a suitable output format for '{Path.GetRandomFileName()}' {Path.GetRandomFileName()}: Invalid argument";

        //Act
        var exception = Record.Exception(() => FFmpegExceptionCatcher.CatchFFmpegErrors(output, args));

        //Assert
        exception.Should().Satisfy<FFmpegNoSuitableOutputFormatFoundException>(Assertion);
        return;

        void Assertion(FFmpegNoSuitableOutputFormatFoundException ex)
        {
            ex.Arguments.Should().Be(args);
            ex.Message.Should().Be(output);
        }
    }
}

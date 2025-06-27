namespace Clearline.MediaFlow.Tests;

using System.Text;
using System.Text.Json;
using Probe.Converters;

public sealed class NullableTimeSpanConverterTests
{
    public static TheoryData<string, TimeSpan> ParseTimeSpanTestCases =>
        new()
        {
            { "01:29:43.253000000", 1.Hours().And(29.Minutes().And(43.Seconds().And(253.Milliseconds()))) },
            { "5688.704000", 1.Hours().And(34.Minutes().And(48.Seconds().And(704.Milliseconds()))) },
        };

    [Theory]
    [MemberData(nameof(ParseTimeSpanTestCases))]
    public void Convert_ReturnsExpectedResult(string input, TimeSpan expected)
    {
        // Arrange
        var converter = new NullableTimeSpanConverter();
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes($"\"{input}\""));

        // Act
        reader.Read();
        var result = converter.Read(ref reader, typeof(TimeSpan?), null!);

        // Assert
        result.Should().Be(expected);
    }
}

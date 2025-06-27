namespace Clearline.MediaFlow;

public readonly record struct ConversionArgument
{
    private ConversionArgument(string name, string? value, ArgumentPosition position)
    {
        Name = name.Trim();
        Value = $"-{name.TrimStart('-').Trim()} {value?.Trim()} ".Trim();
        Position = position;
    }

    public string Name { get; }

    public string Value { get; }

    public ArgumentPosition Position { get; }

    public bool Equals(ConversionArgument other)
    {
        return Name == other.Name &&
               Position == other.Position &&
               Name is not "-i";
    }

    public override int GetHashCode()
    {
        var hashCode = 495346454;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + Position.GetHashCode();
        return hashCode;
    }

    public static ConversionArgument PostInput(string name)
    {
        return new ConversionArgument(name, value: null, ArgumentPosition.PostInput);
    }

    public static ConversionArgument PostInput<T>(string name, T value)
    {
        return Create(name, value, ArgumentPosition.PostInput);
    }

    public static ConversionArgument PreInput(string name)
    {
        return new ConversionArgument(name, value: null, ArgumentPosition.PreInput);
    }

    public static ConversionArgument PreInput<T>(string name, T value)
    {
        return Create(name, value, ArgumentPosition.PreInput);
    }

    public static ConversionArgument Create(string name, ArgumentPosition position)
    {
        return new ConversionArgument(name, value: null, position);
    }

    private static ConversionArgument Create<T>(string name, T value, ArgumentPosition position)
    {
        var stringValue = string.Format(FFmpegFormatProvider.Instance, "{0}", value);
        return Create(name, stringValue, position);
    }

    private static ConversionArgument Create(string name, string value, ArgumentPosition position)
    {
        return new ConversionArgument(name, value, position);
    }

    private sealed class FFmpegFormatProvider : IFormatProvider, ICustomFormatter
    {
        public static readonly FFmpegFormatProvider Instance = new();

        private FFmpegFormatProvider()
        {
        }

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return arg switch
            {
                TimeSpan timeSpan => ToFFmpeg(timeSpan),
                _ => arg?.ToString() ?? string.Empty,
            };
        }

        public object? GetFormat(Type? formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <summary>
        ///     Returns FFmpeg formatted time.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan" /> to format</param>
        /// <returns>The FFmpeg formated time</returns>
        private static string ToFFmpeg(TimeSpan timeSpan)
        {
            var milliseconds = timeSpan.Milliseconds;
            var seconds = timeSpan.Seconds;
            var minutes = timeSpan.Minutes;
            var hours = (int)timeSpan.TotalHours;

            return $"{hours:D}:{minutes:D2}:{seconds:D2}.{milliseconds:D3}";
        }
    }
}

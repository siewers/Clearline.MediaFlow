namespace Clearline.MediaFlow;

// ReSharper disable InconsistentNaming
public readonly partial record struct FrequencyScale
{
    /// <summary>
    ///     Linear frequency scale.
    /// </summary>
    public static readonly FrequencyScale lin = new("lin");

    /// <summary>
    ///     Logarithmic frequency scale.
    /// </summary>
    public static readonly FrequencyScale log = new("log");

    /// <summary>
    ///     Reverse logarithmic frequency scale.
    /// </summary>
    public static readonly FrequencyScale rlog = new("rlog");
}

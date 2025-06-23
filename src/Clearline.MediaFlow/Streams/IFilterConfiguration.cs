﻿namespace Clearline.MediaFlow;

/// <summary>
///     Stream filter configuration
/// </summary>
[PublicAPI]
public interface IFilterConfiguration
{
    /// <summary>
    ///     Type of filter
    /// </summary>
    string FilterType { get; }

    /// <summary>
    ///     Stream filter number
    /// </summary>
    int StreamNumber { get; }

    /// <summary>
    ///     Filter with name and values
    /// </summary>
    Dictionary<string, string> Filters { get; }
}

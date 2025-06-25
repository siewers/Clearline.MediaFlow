namespace Clearline.MediaFlow;

using System.Text;

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

    // void AddFilter(string name, string? value);
    //
    // void AddFilter(Filter filter);

    /// <summary>
    ///     Filter with name and values
    /// </summary>
    FilterCollection Filters { get; }

    internal void Append(StringBuilder builder);
}
